# Dispara Y Esquiva

NOTA: El siguiente proyecto está basado en un [proyecto del usuario hasanavi](https://github.com/hasanavi/OpenCV-Unity3D-Object-Tracking), a partir del cuál empezó la construcción de nuestro aplicativo.

NOTE: The following project is based on a [project by user hasanavi](https://github.com/hasanavi/OpenCV-Unity3D-Object-Tracking), from which we started to build our project.

## ESPAÑOL

Dispara Y Esquiva es un videojuego realizado por los alumnos [Renato Postigo](https://github.com/Naateri/) y [Jazmine Alfaro](https://github.com/JazmineAlfaro), ambos estudiantes de pregrado cursando el curso de Interacción Humano Computador en la Universidad Católica San Pablo. 

Este videojuego es el proyecto de programación de dicho curso dictado en el semestre 2020-1.

## Porción multijugador

El juego ahora incluye una porción multijugador. Es necesario especificar las IPs del servidor en clientPlayer2.cs y del cliente en serverPlayer1.cs. El jugador 1 (servidor, sandía verde) deberá jugar utilizando el archivo hand+colorsMP1.py, mientras que el jugador 2 (cliente, sandía rosada) deberá hacerlo con el archivo hand+colorsMP2.py. 

Se debe tomar en cuenta que si se quiere conectar dos máquinas que no estén en la misma red, se debe utilizar una red VPN. En el caso de los creadores de este juego se utilizó [Radmin VPN](https://www.radmin-vpn.com/) para realizar dicha conexión.

Los videos de la evolución del juego, como se menciona más adelante, se pueden ver en la siguiente carpeta de [Google Drive](https://drive.google.com/drive/u/1/folders/1rCAm8Pl8r9raPbC-vpR5URkj64kXJDYR), ordenados por semana. Además, hay videos mostrando la interacción con el juego en distintos momentos de su evolución.

En esta porción del juego se agrega un cuarto enemigo que se debe destruir para pasar de nivel (nivel 1: solo enemigo 1; nivel 2: enemigos 1 y 2; nivel 3: enemigos 1, 2 y 3). Esto con la idea de promover la colaboración entre ambos usuarios.

También se motiva a visitar el [wiki](https://github.com/Naateri/DisparaYEsquiva/wiki) del repositorio, en el que se discute un poco más a detalle el funcionamiento de algunas partes de esta porción.

Además de esto, el funcionamiento es idéntico a como se describe posteriormente en el siguiente readme. Muchas gracias, y disfrute del videojuego.

## Uso 

Hay dos programas que interactúan entre sí, uno hecho en Unity y otro en Python 3.x.

### Programa en Python

El programa en python tiene dos funciones: usar la palma de la mano para disparar (ponerla dentro del rectángulo y preferiblemente con un fondo sin objetos detrás) y detectar el movimiento de la pantalla de un celular con un color en dicha pantalla.

Para el uso de este programa, son necesarios los siguientes pre-requisitos.

Se necesita tener [pip](https://pip.pypa.io/en/stable/) para instalar las librerías opencv y numpy, utilizadas en el proyecto.

```bash
pip install opencv-python
```

```bash
pip install numpy
```

Además, se necesita de una aplicación en el celular para pintar la pantalla de un solo color. La usada y recomendada por nosotros es [ScreenColor](https://play.google.com/store/apps/details?id=com.techsofts.soft.screencolors&hl=es) para celulares Android. No se han hecho pruebas con celulares iPhone al ninguno de nosotros tener un celular de ese tipo.

El programa a ser utilizado es [hand+colors.py](https://github.com/Naateri/DisparaYEsquiva/blob/master/ImagePrograms/hand%2Bcolors.py), presente en la carpeta ImagePrograms. 

Se probó usando el celular como una cámara web con IP al no tener cámara web propia en la PC utilizada, por lo que si se quiere usar su propia cámara web, se debe cambiar la línea 229 por lo que sigue.

```python
video_font = 0 # o 1, 2, etc. si se tiene más de una cámara web
```

Una vez esté programa se esté ejecutando, se procede a ejecutar el juego en unity.

### Juego en Unity

El juego como tal fue implementado en Unity, y ha sido probado en las versiones 2018.4.20f1 y 2018.4.23f1. 

Es importante notar que la comunicación entre programas fue hecha entre sockets, por lo que a la hora de crear el ejecutable del programa, en Player Settings -> Other Settings -> Configuration tanto en Scripting Runtime Version como en Api Compatibility Level se debe utilizar .NET 4.x.

No es necesario utilizar el ejecutable para poder probar el juego.

## Tutorial

Usando la pantalla de nuestro celular, podemos navegar por el menú. Para escoger una opción, el puntero de color naranja debe permanecer en ese botón por dos o más segundos. Habrá un indicador en la parte inferior izquierda con la cantidad de tiempo encima de un botón además de cuál es dicho botón.

La primera opción es la de jugar, en la que saltaremos a la pantalla de juego. Notaremos que nuestro jugador, una sandía, se mueve según nosotros movamos la pantalla del celular. Para disparar, ponemos nuestra mano en el rectángulo verde que se muestra en pantalla y la abrimos. 

Podemos activar un poder moviendo nuestra pantalla de celular lo más arriba posible, durará 5 segundos y no se podrá reactivar por otros 10 una vez culminado el tiempo. Dicho poder le permitirá al usuario disparar más balas. Además, cada 10 puntos notaremos que el color del marco que envuelve nuestro celular cambiará de color.

Notamos la presencia de tres enemigos: 

* El primero, un cubo de madera. El enemigo simplemente cae, te quita una vida si te golpea y te da un punto si lo destruyes.
* El segundo, un cubo de metal. A diferencia del anterior, si este te golpea te quita dos vidas y se le debe golpear dos veces para destruirlo. Si se le destruye, te da dos puntos.
* El tercero, un cubo de color celeste. Este dispara sus propios proyectiles, los cuáles te quitan una vida. Además, en caso este te golpee, te quita tres vidas. Destruirlo te da tres puntos, y solo se requiere un golpe.

El juego termina cuando perdemos todas nuestras vidas, siendo regresados al menú principal.

En la segunda opción, denominada opciones, podremos escoger la dificultad del juego. La diferencia entre fácil y difícil es que en el segundo caen más objetos en menos tiempo.

En la tercera opción entraremos a un modo de pruebas, en el cuál podremos probar todo lo que trae el juego con vidas ilimitadas. Para volver al menú principal tendremos que hacer click en el botón regresar con el mouse de nuestro computador.

La cuarta opción es para salir del aplicativo.

## Evolución del Proyecto

Desde el inicio del proyecto, semanalmente se andan publicando videos mostrando la evolución del proyecto en la siguiente carpeta de [Google Drive](https://drive.google.com/drive/u/1/folders/1rCAm8Pl8r9raPbC-vpR5URkj64kXJDYR), ordenados por semana. Esto con la intención de poder mostrar como las ideas de nuestro juego fueron cambiando y siendo mejoradas con el tiempo.

Además de esto, se podrá ver ejemplos de jugabilidad dentro del juego, aclarando dudas respecto a cómo jugar en caso aún las haya después de probar el juego.

## ENGLISH

Dispara Y Esquiva (Shoot and Evade) is a videogame developed by [Renato Postigo](https://github.com/Naateri/) and [Jazmine Alfaro](https://github.com/JazmineAlfaro), both undergraduate students at Universidad Católica San Pablo, taking the Human-Computer Interaction course.

This videogame is the programming proyect of said course at semester 2020-1.

## Multiplayer portion

The game now includes a multiplayer portion. It is necessary to specify the server's IP address on clientPlayer2.cs and the client's one on serverPlayer1.cs. Player 1 (server, green watermelon) must play using the hand+colorsMP1.py file, whilst player 2 (client, pink watermelon) must play using the hand+colorsMP2.py file. 

If you want to connect to computers that are not on the same network (i.e. same LAN), a VPN must be used. When testing the game, [Radmin VPN](https://www.radmin-vpn.com/) was used by both game developers.

Videos displaying the game's evolution, as mentioned further ahead, can be seen in the following [Google Drive](https://drive.google.com/drive/u/1/folders/1rCAm8Pl8r9raPbC-vpR5URkj64kXJDYR) folder, sorted by week. There are also videos showing the interaction with the game on different versions of it.

There is, only on this portion of the game, a fourth enemy that must be destroyed to beat the current level (level 1: only enemy 1; level 2: enemies 1 and 2; level 3: enemies 1, 2 and 3). This was made with the intention to promote colaboration between both users.

It is also encouraged to visit the repository's [wiki](https://github.com/Naateri/DisparaYEsquiva/wiki), where there's a more detailed discussion of some parts of the inner-workings of this portion (SPANISH ONLY).

Besides the mentioned in this section, the game works as it is described up next in this readme file. Thank you very much, and enjoy the videogame.

## Usage

There are two programs that interact with each other, one made at Unity and the other at Python 3.x.

### Python Program

The program made using python has two functions: using the palm of your hand to shoot (put it inside the rectangle, preferably with an object-less background behind) and detecting the movement of a cellphone screen, where said screen will have a color assigned to it.

To use this program, the following requirements are needed.

[pip](https://pip.pypa.io/en/stable/) is needed to install both opencv and numpy libraries, used at this project.

```bash
pip install opencv-python
```

```bash
pip install numpy
```

Besides that, an application needed to paint a cellphone screen with only one color is needed. Used and recommended by us is [ScreenColor](https://play.google.com/store/apps/details?id=com.techsofts.soft.screencolors&hl=es) for Android phones. No tests have been made using iPhones.

The program to be used is [hand+colors.py](https://github.com/Naateri/DisparaYEsquiva/blob/master/ImagePrograms/hand%2Bcolors.py), which can be found at the ImagePrograms folder.

This program was tested using the cellphone as an IP webcam due to not having a webcam on the used PC, so if you want to use your own webcam line 229 has to be changed as follows. 

```python
video_font = 0 # o 1, 2, etc. si se tiene más de una cámara web
```

Once this program is running we proceed to execute the game at Unity.

### Game at Unity

The game itself was implemented at Unity, and has been tested at versions 2018.4.20f1 y 2018.4.23f1. 

It's important to note that communication between programs was made between sockets, so when building the program executable, on Player Settings -> Other Settings -> Configuration on both Scrpting Runtime Version and Api Compatibility Level we must use .NET 4.x.

It's not necessary to build an executable to try the game out.

## Tutorial

Using our phone's screen we can navigate through the menu. To choose an option, the orange pointer must stay on said button for two or more seconds. On the bottom left part of the screen there'll be an indicator telling us the amount of time we've been on a button and also which button we're on at that moment.

First button is "play", on where we'll jump to the main game screen. We'll note that our player, a watermelon, will move accordingly to us moving the cellphone. To shoot, we put our hand in the green rectangle shown at screen and we open it, showing our palm.

We can activate a power moving our cellphone as high as possible. Said power will last 5 seconds and won't be able to be reactivated for 10 once it's over. This power will allow the user to shoot more bullets. Also, every 10 points we'll note that the color of the frame wrapping our cellphone will change colors.

We notice the presence of three enemies.

* First one, a wooden cube. The enemy simply falls, takes a life point if it hits you and gives you one point if destroyed.
* Second one, a metal cube. If this one hits you it takes away two life points, and you must hit it twice to destroy it. If destroyed, two points are awarded.
* Third one, a cube light-blue colored. This one can shoot it's own projectiles, which when hitting the user will take away one life point. If this cube hits you it'll take away three life points, as it falls slower than the previous ones. Destroying it awards three points, and only requires one hit.

The game ends when the user looses all it's life points, and the game will show the main menu.

Second option is called "options", where we'll be able to choose the game difficulty. Difference between easy and hard is that in the latter more objects will fall in less time.

Third option will allow us to enter a test mode, on which we'll be able to test the game itself with unlimited life points. To go back to the main menu we'll have to click on the button present at the top right corner using our computer's mouse.

Fourth option is to exit the game.

## Project Evolution

Since the project began, weekly videos have been published showing the project's evolution in the following [Google Drive](https://drive.google.com/drive/u/1/folders/1rCAm8Pl8r9raPbC-vpR5URkj64kXJDYR) folder, ordered by week. The purpose of this is being able to show how the game's ideas have been changing and improved through time. 

Besides this, gameplay examples can be shown, solving existing doubts regarding how to play if these still exist after trying the game out.