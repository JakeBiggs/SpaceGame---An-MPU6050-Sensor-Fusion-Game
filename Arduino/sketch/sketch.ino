#include "Wire.h" //Header file for library that allows I2C device communication 

const int board_address=0x68;

int temperature; //Variables for the temperature sensor
int accelerometer_x, accelerometer_y, accelerometer_z; //Accelerometer variables
int gyro_x, gyro_y, gyro_z; //Gyroscope variables
int c = 0;
float AccErrorX, AccErrorY, GyroErrorX, GyroErrorY, GyroErrorZ;
float AccX, AccY, AccZ;
void setup() {
  // put your setup code here, to run once:

  Serial.begin(115200);
  Wire.begin();
  Wire.beginTransmission(board_address); //Starts transmission to the I2C Board
  Wire.write(0x6B); //PWR_MGMT_1 Register
  Wire.write(0);
  Wire.endTransmission(true);


}

void loop() {
  //calculate_IMU_error();
  // put your main code here, to run repeatedly:
  Wire.beginTransmission(board_address);
  Wire.write(0x3B);
  Wire.endTransmission(false);
  Wire.requestFrom(board_address, 14,true); //14 for number of registers, 2 for accel, 2 gyro, 1 temp * 2 registers making 14

  //Wire.read()<<8 | Wire.read() uses bitwise operator meaning two registers are read and stored in the same variable
  accelerometer_x = (Wire.read() << 8 | Wire.read()) / 16384.0 ; //registers 0x3B and 0x3C (High and Low) 
  accelerometer_y =(Wire.read() << 8 | Wire.read()) / 16384.0 ; //registers 0x3D and 0x3E
  accelerometer_z = (Wire.read() << 8 | Wire.read()) / 16384.0 ; //registers 0x3F and 0x40
  temperature = Wire.read()<<8 | Wire.read(); //registers 0x41 (high) and 0x42 (low)
  temperature = temperature/340 + 36.53;
  gyro_x = Wire.read()<<8 | Wire.read(); //registers 0x43 (H) and 0x42 (L)
  gyro_y = Wire.read()<<8 | Wire.read(); //registers 0x45 (H) and 0x44 (L)
  gyro_z = Wire.read()<<8 | Wire.read(); //registers 0x47 and 0x48
  


  Serial.print(String(accelerometer_x)); Serial.print(",");
  Serial.print(String(accelerometer_y)); Serial.print(",");
  Serial.print(String(accelerometer_z)); Serial.print(",");
  Serial.print(String(temperature)); Serial.print(",");
  Serial.print(String(gyro_x)); Serial.print(",");
  Serial.print(String(gyro_y)); Serial.print(",");
  Serial.print(String(gyro_z));
  Serial.println();
  delay(100);
}

void calculate_IMU_error() {
  // We can call this funtion in the setup section to calculate the accelerometer and gyro data error. From here we will get the error values used in the above equations printed on the Serial Monitor.
  // Note that we should place the IMU flat in order to get the proper values, so that we then can the correct values
  // Read accelerometer values 200 times
  while (c < 200) {
    Wire.beginTransmission(board_address);
    Wire.write(0x3B);
    Wire.endTransmission(false);
    Wire.requestFrom(board_address, 6, true);
    AccX = Wire.read() << 8 | Wire.read();
    AccY = Wire.read() << 8 | Wire.read();
    AccZ = Wire.read() << 8 | Wire.read();
    // Sum all readings
    AccErrorX = AccErrorX + ((atan((AccY) / sqrt(pow((AccX), 2) + pow((AccZ), 2))) * 180 / PI));
    AccErrorY = AccErrorY + ((atan(-1 * (AccX) / sqrt(pow((AccY), 2) + pow((AccZ), 2))) * 180 / PI));
    c++;
  }
  //Divide the sum by 200 to get the error value
  AccErrorX = AccErrorX / 200;
  AccErrorY = AccErrorY / 200;
  c = 0;
  // Read gyro values 200 times
  while (c < 200) {
    Wire.beginTransmission(board_address);
    Wire.write(0x43);
    Wire.endTransmission(false);
    Wire.requestFrom(board_address, 6, true);
    gyro_x = Wire.read() << 8 | Wire.read();
    gyro_y = Wire.read() << 8 | Wire.read();
    gyro_z= Wire.read() << 8 | Wire.read();
    // Sum all readings
    GyroErrorX = GyroErrorX + (gyro_x / 131.0);
    GyroErrorY = GyroErrorY + (gyro_y / 131.0);
    GyroErrorZ = GyroErrorZ + (gyro_z / 131.0);
    c++;
  }
  //Divide the sum by 200 to get the error value
  GyroErrorX = GyroErrorX / 200;
  GyroErrorY = GyroErrorY / 200;
  GyroErrorZ = GyroErrorZ / 200;
  // Print the error values on the Serial Monitor
  Serial.print("AccErrorX: ");
  Serial.println(AccErrorX);
  Serial.print("AccErrorY: ");
  Serial.println(AccErrorY);
  Serial.print("GyroErrorX: ");
  Serial.println(GyroErrorX);
  Serial.print("GyroErrorY: ");
  Serial.println(GyroErrorY);
  Serial.print("GyroErrorZ: ");
  Serial.println(GyroErrorZ);
}
