from enum import Enum

class Simbolos(Enum):
    DELIMITADORES_ABERTURA = "{[("
    DELIMITADORES_FECHAMENTO = ")]}"
    OPERADORES = "+-/*^"
    ATRIBUIDOR = "="
    ATRIBUIDOR_DE_REFERENCIA = "@"
    OPERADOR_BINARIO = "-"
    REGEX_VARIAVEIS = "[a-z]"
    REGEX_NUMEROS = "[0-9]"
    REGEX_EXPRESSAO = f"[{this.OPERADORES}]+"
    ATRIBUIDOR_VARIAVEL = "var"