TDE de LFC

Equipe:
Bruno Cattalini
Felipe Mathieu
Lucas Kaniak

Instruções de uso:
adicionar expressão no arquivo calculo.calc e usá-lo como entrada do programa.
a expressão adicionada precisa estar espaçada: 1 + 2 + 3 + -4

expressão avaliada por linha sem declaração é tipada por uma variável default:
#/ calculo.calc
1 + 2 + 3 + 4
var x = 5 + 6

resultado:
Variavel> VALOR_EXPRESSAO
Valor> 10
Variavel> x
Valor> 11

Bugs conhecidos:
não está reconhecendo variáveis previamente declaradas na árvore, portanto ela não é
avaliada na análise sintática.
ex:
var y = 2 + 2
var x = y + 4
resultado:
y> 4
x> 4

a precedência dos operadores está errada:
1 - 2 * 3 + 4 resulta -7