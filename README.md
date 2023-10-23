# Space Game: A COMP102 Project
Creative Computing Project

# Introduction:
My novel game/experience is called Space Game, a space invaders style game that utilizes the MPU6050, a 6 axis IMU board. To enhance the gaming experience, I have designed a simple but classic controller, a paper airplane, that is easy to use and fits the theme of the game. 


# Game Design:
Space Game will have the player navigating through an asteroid field controlling a spaceship, avoiding oncoming asteroids. The user's aim is to survive for as long as possible and get the highest score that they can.  The game will have a 2.5D style, with some 3D assets. The game will get its data from a C++ Arduino script but the majority of calculations are done within Unity Game Engine in C#.

# Controller Design:
To control the spaceship, I have designed a paper airplane controller that the player can hold and move around. The controller is made out of lightweight material (paper and cardboard) to ensure it is easy to hold and maneuver. It will feature a built-in MPU6050 sensor that will detect the player's movements and translate them into in-game actions.

# Electronic Components:
The key electronic component of the paper airplane controller is the MPU6050 sensor. This sensor combines a 3 axis accelerometer and a 3 axis gyroscope to detect changes in orientation and acceleration. It is then possible to combine these values using mathmatical filters into a final estimation of Orientation. It communicates with the game via extended jumper wires, communicating over serial and transmitting the player's movements in real-time. It connects to the 5V, GND, SCL and SDA pins for communication, power and grounding.

# Ardity
To help with Serial Communication, I made use of Ardity, a free open-sourced Unity Package that allows you to communicate with serial in unity. The source code for this package can be found here: https://github.com/DWilches/Ardity

# Key Challenges Faced
IMU stands for Inertial Measurement Units, and they are boards that use sensor fusion to track physical motion and orientation. Most often they combine an accelerometer an gyroscope (with some more expensive models also using a magnometer or barometer). An accelerometer is a sensor that measures acceleration in one or more directions. A gyroscope on the other hand measures rotational velocity around an axis/axes. 

Neither sensor is great at giving a totally accurate measurment of orientation, so combining them through sensor fusion provides a more comprehensive estimation of attitude. To do this, the accelerometer and gyroscope signals must be filtered and processed. The accelerometer measures linear motion changes, but it is also very sensitive to gravity, meaning it produces alot of noise (or false values). The gyroscope is better at measuring rotational motion, but it can drift by a considerable amount over time.

The biggest challenge I faced in this project was combining the data from multiple sensors to get an accurate orientation tracking system. I implemented both complementary and Kalman filters in order to better estimate the angles, the filters helped to minimize errors and reduce the sensor's drift slightly, resulting in better control of the system. While it required a lot of effort to implement, I believe it was well worth the effort. Although due to scope I had to downsize from all 3 axes being controlled to just 1, as the sensor drift couldnt be fully eliminated.

This reduction in scope allowed me to create a fully working system, rather than one with limited functionality but more ambitious. In future projects, I would allow for more time spent on the complex maths.


# Conclusion:
Our Space Game and paper airplane controller combination offer a unique and engaging gaming experience that will appeal to both casual and hardcore gamers. The space aesthetic of the game and the tactile yet childish nature of the controller will provide an immersive experience that is easy to pick up but difficult to master. The MPU6050 sensor will ensure precise control, and the extended cables will allow players to enjoy the game with some freedom. Overall, the game and controller are a great example of how technology can be used to enhance and improve traditional gaming experiences and show the interface between the digital and physical world.


# References:
http://www.geekmomprojects.com/gyroscopes-and-accelerometers-on-a-chip/
https://github.com/jrowberg/i2cdevlib
https://github.com/DWilches/Ardity
https://www.geekmomprojects.com/wp-content/uploads/2022/03/filter.pdf
https://github.com/kriswiner/MPU6050
https://www.intechopen.com/chapters/63164
https://www.youtube.com/watch?v=qmd6CVrlHOM
https://www.youtube.com/watch?v=hQUkiC5o0JI
