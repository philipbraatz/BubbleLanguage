grammar Bubbles;

/*
 * Parser Rules
 */
 /*
 bubble_file : bubble_decleration*
 */
scope_type: PUBLIC | PRIVATE | INTERNAL;

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
	(':' structure? imports )? (as_type (',' as_type)*)? '{' 
		   (space_decleration | class_decleration | interface_decleration | function_decleration)*
	'}';

class_decleration :
	scope_type? CLASS NAME
	(':' structure? imports )? (as_type (',' as_type)*)? '{' 
		   (constructor_bubble | property_bubble | methods_bubble )*
	'}';
	
interface_decleration :
	scope_type? INTERFACE NAME
	(':' structure? imports )? (as_type (',' as_type)*)? '{' 
		   (constructor_bubble | property_bubble | methods_bubble )*
	'}';

function_decleration :
	scope_type? FUNCTION NAME
	(':' structure? imports )? (as_type (',' as_type)*)? '{' 
		   code_lines?
	'}';

constructor_bubble : CONSTRUCTOR NAME? '{' '}';
property_bubble: scope_type PROPERTY NAME? as_type? '{' '}';
methods_bubble: scope_type METHOD NAME? as_type? '{' '}';
	

/*scope_bubble: scope_type '{' (bubble_decleration | )* '}';*/
function: FUNCTION structure? NAME parameters? (':'code_line | '{' (code_lines)? '}');

structure:  ('<' NAME (','NAME)* '>');
imports: (USING | FROM) import_name (','import_name)*;
import_name: class_name ('#'  NAME)?;
parameters: '('((param',')* param)?')';
param: NAME NAME ('=' NAME)?;

class_name: NAME ('.' NAME)*;
code_line: class_name;
code_lines: code_line+;


/*
 * Lexer Rules
 */
 /*
 */
WS  : (' '|'\t'|'\r'|'\n')+ -> skip;

SPACE : 'space' | 'namespace' | 'sp';
CLASS : 'class' | 'cl';
FUNCTION : 'function' | 'func' |'fn';
INTERFACE: 'interface' | 'it';

CONSTRUCTOR: 'constructor' | 'con' | 'init' | 'initialize' | 'initializer';
METHOD: 'methods' | 'functions' | 'funcs';
PROPERTY: 'properties' | 'property' | 'prop';

PUBLIC: 'public' | 'pub';
PRIVATE: 'private' | 'pvt';
INTERNAL: 'internal' | 'inl';

STATIC: 'static' | 'tic';
EVENT: 'event' | 'ent';
ASYNC: 'asyncronous' | 'async' | 'ac';

USING: 'using' | 'use';
FROM: 'FROM';


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

