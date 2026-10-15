#include <Servo.h>

// --- OUTPUT PINNEN (Vanuit Unity) ---
const int buzzerPin = 7;
const int relayPin = 8;
const int servo180Pin = 9;
const int contServoAPin = 10;
const int contServoBPin = 11;
const int ledPin = 13; 

// --- INPUT PINNEN (Naar Unity) ---
const int dig1Pin = 2; 
const int dig2Pin = 3; 
const int dig3Pin = 4; 
const int ana0Pin = A0; 
const int ana1Pin = A1; 

Servo servo180;
Servo contServoA;
Servo contServoB;

int lastDig1 = HIGH, lastDig2 = HIGH, lastDig3 = HIGH;
int lastAna0 = -1, lastAna1 = -1;

void setup() {
  Serial.begin(9600);
  
  pinMode(buzzerPin, OUTPUT);
  pinMode(relayPin, OUTPUT);
  pinMode(ledPin, OUTPUT);
  
  // Activeer interne weerstanden voor de 3 digitale pinnen
  pinMode(dig1Pin, INPUT_PULLUP);
  pinMode(dig2Pin, INPUT_PULLUP);
  pinMode(dig3Pin, INPUT_PULLUP);

  servo180.attach(servo180Pin);
  contServoA.attach(contServoAPin);
  contServoB.attach(contServoBPin);

  digitalWrite(relayPin, LOW);
  digitalWrite(buzzerPin, LOW);
  digitalWrite(ledPin, LOW);
  servo180.write(0);
  contServoA.write(90);
  contServoB.write(90);
}

void loop() {
  // --- ONTVANGEN VAN UNITY ---
  if (Serial.available() > 0) {
    char command = Serial.read();

    if (command == 'M') digitalWrite(relayPin, HIGH); 
    if (command == 'm') digitalWrite(relayPin, LOW);  
    if (command == 'L') digitalWrite(ledPin, HIGH);   
    if (command == 'l') digitalWrite(ledPin, LOW);    
    if (command == 'O') servo180.write(180); 
    if (command == 'D') servo180.write(0);   
    if (command == 'F') contServoA.write(180); 
    if (command == 'R') contServoA.write(0);   
    if (command == 'S') contServoA.write(90);  
    if (command == 'B') digitalWrite(buzzerPin, HIGH); 
    if (command == 'b') digitalWrite(buzzerPin, LOW);  
  }

  // --- VERSTUREN NAAR UNITY ---
  int curDig1 = digitalRead(dig1Pin);
  if (curDig1 != lastDig1) { Serial.print("DIG1:"); Serial.println(curDig1 == LOW ? "1" : "0"); lastDig1 = curDig1; }

  int curDig2 = digitalRead(dig2Pin);
  if (curDig2 != lastDig2) { Serial.print("DIG2:"); Serial.println(curDig2 == LOW ? "1" : "0"); lastDig2 = curDig2; }

  int curDig3 = digitalRead(dig3Pin);
  if (curDig3 != lastDig3) { Serial.print("DIG3:"); Serial.println(curDig3 == LOW ? "1" : "0"); lastDig3 = curDig3; }

  int curAna0 = analogRead(ana0Pin);
  if (abs(curAna0 - lastAna0) > 5) { Serial.print("ANA0:"); Serial.println(curAna0); lastAna0 = curAna0; }

  int curAna1 = analogRead(ana1Pin);
  if (abs(curAna1 - lastAna1) > 10) { Serial.print("ANA1:"); Serial.println(curAna1); lastAna1 = curAna1; }

  delay(10); 
}