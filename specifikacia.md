# PPchat - špecifikácia

## Krátky popis

Tri časti:

0) PPparsing - (C++) knižnica na parsovanie jednoduchých príkazov s argumentami
1) PPnetwork - knižnica pre vytváranie sieťových aplikácií
2) PPchat - TCP klient-server aplikácia s jednoduchým "četovaním"

### PPparsing

Knižnica v C++.

PPnetwork ju používa pre "rýchlejšie" parsovanie príkazov.

### PPnetwork

Knižnica pre .NET.

Veľký dôraz na abstrakciu.

Ponúka základ pre aplikácie prijímajúce príkazy s argumentami cez textový vstup
a sieťové prepojenia a komunikáciu medzi týmito aplikáciami.

Základnou jednotkou komunikácie medzi:
* koncovým užívateľom a aplikáciou - príkaz
* dvoma aplikáciami - packet

Pre pridanie príkazov a packetov používa reflexiu,
aby takéto rozširovanie o funkcie bolo čo najjednoduchšie
pre užívateľa knižnice.

Ponúka aj jednoduchú serializáciu do Streamu.

### PPchat

Jednoduchý "četovací" klient-server projekt demonštrujúci využitie PPnetwork.

## Požiadavky

### NPRG038

#### Vlákna

PPnetwork - každé pripojenie prijíma packety na samostatnom vlákne

PPchat - server prijíma nové pripojenia na samostatnom vlákne

#### Ostatné

Generické typy, LINQ, Extension metódy, iterator metódy

### NPRG057

#### P/Invoke

PPnetwork - volá PPparsing (C++)

#### Ostatné

pointery, fixed statement
