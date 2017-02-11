## Project 1-3 Spec [Due 2/16]:

### How to get the project

1. Clone the repo with 
  ``git clone https://github.com/berkeleyGamedev/Project1-3.git``
2. Check Piazza for bug fixes!

### About the Project

This project is for teams of 1-2 (we would prefer teams of 2). Every team must have at least 1 programmer. Every team must do the Editor and Programming sections, and the Art section is optional.

### Grade Breakdown:

Requirements: 100%

### Project Specification

**A note on UIManager:** ``UIManager`` uses the **Singleton** pattern. This pattern declares a static member variable to be the single instance of the class, so that it can be accessed globally through ```UIManager.uiManager``` (``uiManager`` is the name of our static member variable). In ``Start()``, we enforce this pattern by checking if an instance already exists, and destroying any new ``UIManager`` objects that are created. 

**A note on SceneManager.LoadScene:** ``SceneManager.LoadScene(string scene name)`` would be the normal way to load a scene in Unity. For "Main Scene" (our World 1-1 scene), we have abstracted it in ``UIManager.LoadScene()`` to make sure that the Loading Screen shows up. When you want to reload the Main Scene, please use ```UIManager.LoadScene()```, but the menu can be reloaded in the normal way.

#### Unity Editor:  

1. In the 1Player GameObject, make the button component interact with ``LoadOnClick`` component attached to its parent.

#### Programming:  

1. UIManager.cs
   1. ``TakeLife()``
1. LoadOnClick.cs
   1. ``LoadScene()``
1. PlayerController.cs
   1. ``Shrink()`` 

### Submission:

* Place a copy of all your art files into a separate folder.

* Make sure you have added us as collaborators

* Add a commit with a final submission tag
