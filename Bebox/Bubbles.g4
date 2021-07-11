grammar Bubbles;

/*
 * Parser Rules
 */
 /*
 bubble_file : bubble_decleration*
 */
scope_type: PUBLIC | PRIVATE | INTERNAL;

bubble_type : SPACE | CLASS | INTERFACE | FUNCTION;

as_type : 'as' (STATIC | EVENT | ASYNC);

file : bubble_decleration* EOF;

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
bubble_decleration : 
	scope_type? bubble_type? NAME?
	(':' structure? imports )? (as_type (',' as_type)*)? '{' 
		   (code_lines | bubble_decleration | function )*
	'}';

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

PUBLIC: 'public' | 'pub';
PRIVATE: 'private' | 'prv';
INTERNAL: 'internal' | 'inn';

STATIC: 'static' | 'tic';
EVENT: 'event' | 'ent';
ASYNC: 'asyncronous' | 'async' | 'ac';

USING: 'using';
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

