from abc import ABC

class Operando():
    def __init__(self, valor: int):
        self.valor = valor

    def __init__(self, valor: float):
        self.valor = valor

    def __init__(self, valor: str):
        self.valor = valor


class Operacao(ABC):
    def __init__(self, operador1:Operando, operacao:str, operador2:Operando):
        self.operador1 = operador1
        self.operador2 = operador2
        self.operacao = operacao

    @abstractmethod
    def aplicar(self):
        pass

    @abstractmethod
    def printMe(self):
        pass

class Adicao(Operacao):
    def aplicar(self):
        return self.operador1 + self.operador2;

    def printMe(self):
        return f'add ( {self.operador1}, {self.operador2} ) '

class Subtracao(Operacao):
    def aplicar(self):
        return self.operador1 - self.operador2;

    def printMe(self):
        return f'sub ( {self.operador1}, {self.operador2} ) '

class Multiplicacao(Operacao):
    def aplicar(self):
        return self.operador1 * self.operador2;

    def printMe(self):
        return f'mul ( {self.operador1}, {self.operador2} ) '

class Divisao(Operacao):
    def aplicar(self):
        return self.operador1 / self.operador2;

    def printMe(self):
        return f'div ( {self.operador1}, {self.operador2} ) '

class Negativo(Operacao):
    def aplicar(self):
        return self.operador1 * -1;

    def printMe(self):
        return f'inv ( {self.operador1} ) '
