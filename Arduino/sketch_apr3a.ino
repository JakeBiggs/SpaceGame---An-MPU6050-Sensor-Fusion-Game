#include "Wire.h" //Header file for library that allows I2C device communication 

const int board_address=0x68;

int temperature; //Variables for the temperature sensor
int accelerometer_x, accelerometer_y, accelerometer_z; //Accelerometer variables
int gyro_x, gyro_y, gyro_z; //Gyroscope variables


void setup() {
  // put your setup code here, to run once:

  Serial.begin(9600);
  Wire.begin();
  Wire.beginTransmission(board_address); //Starts transmission to the I2C Board
  Wire.write(0x6B); //PWR_MGMT_1 Register
  Wire.write(0);
  Wire.endTransmission(true);
}

void loop() {
  // put your main code here, to run repeatedly:
  Wire.beginTransmission(board_address);
  Wire.write(0x3B);
  Wire.endTransmission(false);
  Wire.requestFrom(board_address, 14,true); //14 for number of registers, 2 for accel, 2 gyro, 1 temp * 2 registers making 14

  //Wire.read()<<8 | Wire.read() uses bitwise operator meaning two registers are read and stored in the same variable
  accelerometer_x = Wire.read()<<8 | Wire.read(); //registers 0x3B and 0x3C (High and Low) 
  accelerometer_y = Wire.read()<<8 | Wire.read(); //registers 0x3D and 0x3E
  accelerometer_z = Wire.read()<<8 | Wire.read(); //registers 0x3F and 0x40
  temperature = Wire.read()<<8 | Wire.read(); //registers 0x41 (high) and 0x42 (low)
  gyro_x = Wire.read()<<8 | Wire.read(); //registers 0x43 (H) and 0x42 (L)
  gyro_y = Wire.read()<<8 | Wire.read(); //registers 0x45 (H) and 0x44 (L)
  gyro_z = Wire.read()<<8 | Wire.read(); //registers 0x47 and 0x48

  //prints data
  Serial.print("aX = "); Serial.print(String(accelerometer_x));
  Serial.print(" | aY = "); Serial.print(String(accelerometer_y));
  Serial.print(" | aZ = "); Serial.print(String(accelerometer_z));
  // the following equation was taken from the documentation [MPU-6000/MPU-6050 Register Map and Description, p.30]
  Serial.print(" | tmp = "); Serial.print(temperature/340.00+36.53);
  Serial.print(" | gX = "); Serial.print(String(gyro_x));
  Serial.print(" | gY = "); Serial.print(String(gyro_y));
  Serial.print(" | gZ = "); Serial.print(String(gyro_z));
  Serial.println();
  delay(1000);
}
