grammar Bubbles;

/*
 * Parser Rules
 */
 /*
 bubble_file : bubble_decleration*
 */
scope_type: PUBLIC | PRIVATE | PROTECTED;

bubble_type : SPACE | CLASS | INTERFACE | FUNCTION | CONSTRUCTOR;

as_type : 'as' (STATIC | EVENT | ASYNC);

file : space_decleration* EOF;

/*
Space MyNamespace: 
using 
	systemStuff,
	otherCoolLibrary = simpleName
{
	Public Class Main
		as static
	{
		OnlyValidWritableCodeRNis1Word
	}

	Public Class MyClass: <T1, otherTemplateClass> 
		from MyParentClass, MaybeSomeotherClass
	{
		TestCodeHere
	}
}
*/


space_decleration : 	
	scope_type? SPACE NAME?
	(as_type (',' as_type)*)? (':' structure? imports )?  '{' 
		   (space_decleration | class_decleration | interface_decleration | function_decleration)*
	'}';

class_decleration :
	scope_type? CLASS NAME
	inherit_class?
	'{' 
		   (constructor_bubble | property_bubble | methods_bubble )*
	'}';
	
interface_decleration :
	scope_type? INTERFACE NAME
	(as_type (',' as_type)*)? (':' structure? imports )? '{' 
		   (constructor_bubble | property_bubble | methods_bubble )*
	'}';

function_decleration :
	return_values? FUNCTION NAME
	parameters? (':' structure? as_type )?  '{' 
		   code_line*
	'}';

constructor_decleration: CONSTRUCTOR
	( param | '(' (param (',' param)*)? ')')? (':' structure? imports )? '{' 
		   code_line*
	'}';
property_decleration: CLASS_NAME NAME (':' expression)?;/*TODO create property logic*/

constructor_bubble : scope_type CONSTRUCTOR NAME? '{' constructor_decleration* '}';
property_bubble: scope_type PROPERTY NAME? as_type? '{' property_decleration* '}';
methods_bubble: scope_type METHOD NAME? as_type? '{' function_decleration* '}';
	

structure:  '<' NAME (','NAME)* '>';
imports: (USING | 'from') import_name (','import_name)*;
import_name: CLASS_NAME ('#'  NAME)?;
parameters:  '(' (param (',' param)*)? ')';
param: NAME NAME ('=' NAME)?;
return_values: VOID | NAME | ('[' NAME (','NAME)* ']');

inherit_class: 'from' CLASS_NAME;

code_line: expression;
code_lines: code_line+;

expression: 
	expression (MULT | DIVIDE) expression
	| expression (ADD | SUB) expression
	| RETURN expression
	| CLASS_NAME
	;

/*
 * Lexer Rules
 */
 /*
 */

COMMENT : '//' ~[\r\n]* -> channel(HIDDEN);
COMMENTS : '/*' (options {greedy=false;} : .* ) '*/' -> channel(HIDDEN);
WS  : (' '|'\t'|'\r'|'\n')+ -> skip;


/*Bubbles*/

SPACE : 'space' | 'namespace' | 'sp';
CLASS : 'class' | 'cl';
FUNCTION : 'function' | 'func' |'fn';
INTERFACE: 'interface' | 'it';

CONSTRUCTOR: 'constructor' | 'con' | 'init' | 'initialize' | 'initializer';
METHOD: 'methods' | 'functions' | 'funcs';
PROPERTY: 'properties' | 'property' | 'prop';

PUBLIC: 'public' | 'pub';
PRIVATE: 'private' | 'pvt';
PROTECTED: 'protected' | 'prot';

STATIC: 'static' | 'tic';
EVENT: 'event' | 'ent';
ASYNC: 'asyncronous' | 'async' | 'ac';

/*End Bubbles*/

USING: 'using' | 'use';

RETURN: 'return' | '=>';
VOID: 'void' | 'null' | '_';

/*Operators*/

/*equality*/
EQUAL: '==';/*Normally equality*/
NOT_EQUAL: '!=' | '<>';
SAME:'===';/*are these the same instance of object*/
NOT_SAME: '!==' | '=!=';
LEFT_GREATER: '<';
RIGHT_GREATER: '>';
LEFT_EQUAL_GREATER: '<=';
RIGHT_EQUAL_GREATER: '>=';
IS_TYPE: 'is';

/*Assignment*/
ASSIGN: '=';
INC_ASSIGN: '+=';
DEC_ASSIGN: '-=';
MULT_ASSIGN: '*=';
DIVIDE_ASSIGN: '/=';
INVERT_ASSIGN: '!!=';

/*Logic*/
NOT: '!' | 'not';
AND: '&&' | 'and';
OR: '||' | 'or';
NAND: '!&&' | 'nand';
XOR: 'x||' | 'xor';
NOR: '!||' | 'nor';

/*Math*/
INC: '++';
DEC: '--';

ADD: '+';
SUB: '-';
MULT: '*';
DIVIDE: '/';
MOD: '%' | 'mod' | 'remainder';
POWER: '^';

/*Nullablity*/
SKIP_IF_NULL: '?';
IF_NULL: '??';

INLINE_IF: '?=';
INLINE_ELSE: '|=';

/*End Operators*/


fragment NONZERONUM : [1-9];
fragment NUM : [0-9];


fragment INT
   : '0' | [1-9] [0-9]*
   ;

fragment EXP
   : [Ee] [+\-]? INT
   ;

NUMBER
   : '-'? INT ('.' NUM +)? EXP?
   ;


NAME : [a-z] ([a-z] | [0-9])*;

CLASS_NAME:  [a-z] ([a-z] | [0-9] | '.')*;

STRING: '"' .*? '"';