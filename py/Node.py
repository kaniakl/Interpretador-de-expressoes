from abc import ABC, abstractmethod
from Expressoes import ExpressaoSemantica
from Operacoes import Operacao


class Node(ABC):
    def __init__(self):
        self.esquerda = None
        self.direita = None
        self.valor = "";

    def __init__(self, esquerda: Node, direita: Node, valor: str):
        self.esquerda = esquerda
        self.direita = direita
        self.valor = valor

    def __init__(self, esquerda: Node, direita: Node, valor: Operacao):
        self.esquerda = esquerda
        self.direita = direita
        self.valor = valor

    @abstractmethod
    def Traverse():
        pass


class NodeSintaxe(Node):
    def __init__(self, esquerda: Node, direita: Node, valor: str):
        self.esquerda = esquerda
        self.direita = direita
        self.valor = valor

    def Traverse():
        if self.esquerda is None or self.direita is None:
            return self.valor
        if self.esquerda is None:
            return self.valor + Traverse(self.direita)
        if self.direita is None:
            return Traverse(self.esquerda) + self.valor
        return Traverse(self.esquerda)

class NodeSemantica(Node):
    def __init__(self, esquerda: Node, direita: Node, valor: Operacao):
        self.esquerda = esquerda
        self.direita = direita
        self.valor = valor

    # aqui tem que fazer diferente
    def Traverse():
        pass