# Hybride Werelden - Arduino & Unity Template

Welkom bij het basisproject voor de minor Hybride Werelden! Met deze template koppel je zonder te programmeren fysieke knoppen, sensoren en motoren (via een Arduino) aan je digitale Unity game.

Dit project bevat een "Zwitsers Zakmes" script waarmee alles via handige menu's in de Unity Inspector werkt.

## 1. Eerste keer instellen (Unity)

Voordat je begint, moet Unity toestemming hebben om via USB te communiceren.

1. Ga in Unity naar `Edit` > `Project Settings` > `Player`.
2. Klap `Other Settings` open.
3. Scroll naar `Api Compatibility Level` en zet deze op **.NET Framework**.
   *(Let op: Unity moet nu even laden, dit is normaal!)*

## 2. Hardware Aansluiten (Arduino)

Upload de meegeleverde code via de Arduino IDE naar je Arduino Uno. Sluit de componenten aan zoals op de onderstaande foto's. Zorg ervoor dat je de bijbehorende afbeeldingen in de map `assets` plaatst op GitHub.

### Outputs (Unity bestuurt de echte wereld)

Unity kan commando's sturen naar de Arduino om dingen te laten bewegen of geluid te laten maken.

* **Pin 7:** Buzzer
* **Pin 8:** Relais (voor bijv. een grote motor)
* **Pin 10 & 11:** 360-graden (Continuous) Servo's

**Pin 9: 180-graden Servo**
![180-graden Servo](assets/Screenshot%202026-10-16%20at%2010.13.09.png)

**Pin 13: LED Lampje**
![LED Lampje](assets/Screenshot%202026-10-16%20at%2010.12.56.jpg)

### Inputs (De echte wereld bestuurt Unity)

Fysieke knoppen en sensoren sturen automatisch data naar Unity.

#### Digitale Inputs (Aan/Uit)

Voor sensoren zoals Microswitches, knoppen of infrarood-poortjes.

**Pin 2: Microswitch**
![Microswitch](assets/Screenshot%202026-10-16%20at%2010.12.22.png)

**Pin 4: Infrarood Sensor**
![Infrarood Sensor](assets/Screenshot%202026-10-16%20at%2010.12.34.png)

#### Analoge Inputs (Glijdende waardes)

Voor sensoren met waardes van 0 tot 1023.

**Pin A0: Potmeter (Draaiknop)**
![Potmeter](assets/Screenshot%202026-10-16%20at%2010.11.56.png)

**Pin A1: LDR (Lichtsensor)** 
*(Let op: op de foto aangesloten op A0, maar in ons script standaard op A0 of A1 te gebruiken)*
![LDR Lichtsensor](assets/Screenshot%202026-10-16%20at%2010.12.47.png)

## 3. Werken in Unity (No-Code!)

Zodra je hardware is aangesloten en de Arduino in de USB-poort zit, kun je in Unity aan de slag.

1. Zoek het object **ArduinoManager** in de Hierarchy en klik erop.
2. Je ziet nu in de Inspector alle in- en uitgangen van de Arduino.
3. **Automatisch Verbinden** staat standaard aan. Unity zoekt zelf de juiste poort (werkt dit niet? Vink het uit en vul handmatig je poort in, bijv. `COM3` op Windows of `/dev/cu.usbmodem1234` op Mac).

### Hoe koppel ik een knop?

Klik bij `Digitale Input 1 (Pin 2)` onder **On Digital 1 Aan ()** op het plusje. Sleep het object dat je wilt besturen erin, kies een functie en je bent klaar!

### Hoe gebruik ik een analoge sensor?

Voor analoge sensoren (A0 en A1) kun je in de Inspector een **Drempelwaarde** instellen (standaard 512).

* Gebruik **Boven Drempel / Onder Drempel** om de sensor als een simpele aan/uit schakelaar te gebruiken.
* Zet **Print Debug** aan om de exacte waardes in je Unity Console te zien tijdens het testen.