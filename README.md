# Smart Space Journey, made with Unity
**Project Title: Smart Space Journey**  
This project, developed using Unity, combines artificial intelligence (AI) with physics-based control to navigate a spaceship in a complex space environment, with the added challenge of handling asteroids and enemy UFOs. The AI is trained using reinforcement learning techniques to navigate and make decisions autonomously. You can see  it more visually in this [presentation](Smart%20Space%20Journey.pdf).

### Description:
The main focus of this project is the **Automatic Navigation AI**, which propels the spaceship through space using forces. The AI is trained to handle movement along the X, Y, and Z axes, avoiding obstacles and adapting to various challenges such as dodging asteroids, destroying them with missiles, and chasing UFOs. It integrates several algorithms, including position tracking and height-reaching for precise navigation, and physics-based control for realistic movement.

The project incorporates a detailed environment with elements like large asteroids, connected asteroid groups, and small asteroids that the AI has to navigate around. Additionally, **UFOs** are generated randomly, introducing further complexity to the simulation, as the AI must engage with them using homing missiles guided by specialized algorithms.

### Challenges:
- **AI Training**: One of the key challenges was training the AI to predict and adjust its movement. The system had to learn to minimize errors and control variables like acceleration and rotation to optimize its path. Additionally, teaching the AI to use physical forces for propulsion, while adhering to speed limits, was a significant hurdle.
- **Real-Time Decision Making**: The AI constantly processes environmental data, such as the position of asteroids and UFOs, to make decisions. It must decide when to dodge, fire missiles, or change its trajectory, ensuring it can react in real time.
- **Physics-Based Control**: Integrating realistic physical forces for the spaceship's movement and missile trajectory was crucial for creating a believable environment. Ensuring that the spaceship and missiles behaved naturally while reacting to different forces was technically demanding.

### Results:
The **Navigation AI** successfully learned to navigate the environment autonomously, adapting to the various space hazards. The AI-driven spaceship can:
- Avoid or destroy large asteroids using missiles.
- Dodge small asteroids by calculating their relative position.
- Pursue and eliminate UFOs using guided missiles that track position and altitude.

### Benefits:
- **Autonomous Navigation**: The AI system provides a fully autonomous spaceship capable of decision-making, reducing the need for manual control in space environments.
- **Realistic Physics Simulation**: The use of physics-based controls enhances the realism of the simulation, providing an immersive space experience.
- **Scalable**: The framework can be scaled for more complex space environments, incorporating additional objects or threats for further AI training and refinement.

The combination of AI learning and physics control in this project demonstrates the power of reinforcement learning in autonomous navigation, leading to a sophisticated and interactive space simulation.
