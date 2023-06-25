from Operacoes import Adicao, Subtracao, Divisao, Multiplicacao, Negativo

class ExpressaoSintaxe:
    def __init__(self):
        self.variaveis = []
        self.numeros = []
        self.operadores = []
        self.delimitadores = []
        self.atribuidor = []
        self.acessoValorVariaveis = []
        self.declaracaoVariavel = []
        self.texto = ""

    def __init__(self,
            variaveis=[], numeros=[], operadores=[],
            delimitadores=[], atribuidor=[], acessoValorVariaveis=[],
            declaracaoVariavel=[], texto=""):
        self.variaveis = variaveis
        self.numeros = numeros
        self.operadores = operadores
        self.delimitadores = delimitadores
        self.atribuidor = atribuidor
        self.acessoValorVariaveis = acessoValorVariaveis
        self.declaracaoVariavel = declaracaoVariavel
        self.texto = texto

    def __format__(self):
        return (
          f'variaveis: {" || ".join(self.variaveis)}\n'
          f'numeros: {" || ".join(self.numeros)} \n'
          f'operadores: {" || ".join(self.operadores)} \n'
          f'delimitadores: {" || ".join(self.delimitadores)} \n'
          f'atribuidor: {" || ".join(self.atribuidor)} \n'
          f'acessoValorVariaveis: {" || ".join(self.acessoValorVariaveis)} \n'
          f'declaracaoVariavel: {" || ".join(self.declaracaoVariavel)} \n'
          f'texto: {self.texto} \n'
        )

    def Parse(self, linha: str) -> list(str):
        pass

class ExpressaoSemantica:
    def __init__(self):
        self.resultado = ""

	# expects: node (arvore de expressao)
	# returns: node (arvore contendo a expressaoSemantica)
    def Converter(self, node: Node) -> Node:
        pass

	# expects: node (arvore contendo a expressaoSemantica)
	# returns: str com o resultado da expressao
    def Interpretar(self, node: Node) -> str:
        pass