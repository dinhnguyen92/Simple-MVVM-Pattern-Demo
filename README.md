# Simple-MVVM-Pattern-Demo

A simple demonstration of how the MVVM pattern in .NET can be implemented

Coming from a MVC background, I found the MVVM pattern counter-intuitive and hard to understand at first.
A quick look around forums and posts online seems to suggest that many others have the same problem.
As such, I'm posting this project as well as a brief explanation of MVVM in order to help anyone interested in learning this pattern.

In this README document, I will only give a qualitative explanation of the program and its implementation. 
For more details, please look at the codes.

Please note that I do not presume to be an expert on MVVM. Nor do I claim that my implementation of MVVM is correct.
This project is simply my own interpretation and understanding of MVVM. 
If you're learning MVVM, I hope you would find this useful.


### Program Overview 
The program is a simple GUI with the following elements:
- First name text field: for the user to input first name
- Last name text field: for the user to input last name
- Welcome message text field: output welcome message with the full name
- Save button: used to demonstrate user input validation in MVVM

When the user types in their first and last names, the full name in the welcome message output field will be dynamically changed accordingly.
In order to validate user's input, the save button is disabled if either the first name or last name field is null. The button is only enabled
if the user provides both first name and last name.


### MVVM Implementation
MVVM is a pattern for developing applications with GUI in the .NET platform. MVVM stands for "Model - View - View Model". 
Just like in the MVC pattern (Model - View - Controller), the Model is the business logic and data, and the View is the GUI.
In the MVC pattern, the inputs and commands from the user are processed by the controller before being passed on to the model.
The controller is also responsible for changing the view based on state changes in the model.
In other words, the controller handles the application logic (as opposed to the business/back-end logic). 

In the MVVM pattern, the coordination between the View and the Model is handled by the View Model. 
In .NET, the View Model implements 2-way data-binding between the View and the Model. 
Anytime a feature/field in the View changes, the change is reflected in the corresponding feature/variable in the View Model.
Changes in the View Model are then propagated to the corresponding variables in the Model, thus changing the state of the Model.
This process can also happen in reverse, allowing changes in the Model to propagate to the View via the View Model.

Data-binding between the View and the View Model is implemented in the View's XAML code. 
To bind a field in the View to a variable in the View Model, the variable path is specified in the binding property of the field.
This also works with commmands: a view feature such as a button can be bound to a specific Delegate Command object in the View Model.

Data-binding between the View Model and the Model is implemented using the INotifyPropertyChanged interface.
Each time a variable in the View Model changes, the event PropertyChanged is raised. 
All methods that are subscribed to this event are then invoked. 
These methods are responsible for making the the corresponding changes in the Model.
This same process also happens in the reverse direction whenever a variable in the Model changes.

To demonstrate the data-binding mechanism in MVVM, the program accepts the first name and last name from the user using the View.
The first name and last name are passed by the View to the View Model, which then passes them to the Model.
In the Model, the "business logic" is implemented: the first name and last name are concatenated to form the full name.
The full name produced by the Model is then passed back to the View Model.
In the View Model, the "application/presentation logic" is implemented: a welcome message is crafted using the full name.
The welcome message is then passed by the View Model to the View for display.


### Important Considerations

**Where to raise the PropertyChanged event:**

The best place to raise the PropertyChanged event is inside the setter of a property. 
This practice has several advantages:
- Ensures that the event is raised at the correct time: when the property actually changes (instead of before or after)
- Having a predefined place in the code to raise the event makes sure that you won't forget where you raise it, 
which can make code changes and maintenance less messy
- If the setters and getters are implemented using accessors in C#, there are some derived benefits to be had (explain below)

**Using a single PropertyChanged event for multiple different properties:**

Any useful program almost always has more than one property. 
If an event had to be defined for each property, there would be an explosion of events, which would be a maintenance nightmare.
To solve this problem, the INotifyPropertyChanged interface is used. 

The INotifyPropertyChanged interface implements the PropertyChanged event. 
Each time this event is raised, the name of the specific property is passed to the invoked method as a PropertyChangedEventArgs object.
Inside the invoked method, the name of the property is retrieved, and the method will use this name to decide what to do.
In the past, the name of the changed property must be passed manually by the publisher of the event.
Starting with Visual Studio 2012 and C# 4.5, the CallerMemberNameAttribute class can be used to enable the name of the property to be inferred 
dynamically during runtime.

According to .NET documentation, the CallerMemberNameAttribute "allows you to obtain the method or property name of the caller to the method".
This works particularly well with accessors: when an event is raised inside a setter defined inside an accessor, 
the name of the accessor is passed as the property name along with the event to the invoked method.
This greatly simplifies the task of identifying which property has changed.

**Preventing event bouncing:**

Raising the PropertyChanged event in the setter creates a bug that can be hard to detect: event bouncing.
Suppose the Model has a property called mAge, and the View Model has a property called vmAge. 
Suppose the user changes vmAge via the View, and the PropertyChanged event is raised by the View Model.
In response, the Model changes mAge accordingly. 
However, since the PropertyChanged event is raised whenever the setter of mAge is invoked, the View Model is then notified to change vmAge again.
This results in the "ricocheting" or "bouncing" of events between the View Model and the Model.
On the surface, the change made by the user will persist and be displayed by the View.
In the background, this bouncing of events can go on indefinitely undetected and slow down the application.
More dangerously, asynchronous changes to either vmAge or mAge maybe nullified by event bouncing: 
if, immediately after vmAge is changed to 10 by the user, the event triggered by a previous value of vmAge (let's say 9) bounces back to vmAge,
vmAge will then store the wrong value. This wrong value will then be propagated to mAge via the same event being bounced around.

To solve this problem, each time the setter of a property is invoked in response to a PropertyChanged event,
the new value of the property must be compared to the current value. 
If the values are the same, the event received is a bounced-back event, and no further PropertyChanged event is invoked. 
If setters and getters are implemented inside accessors, C# provides the convenient "value" parameter 
for comparing the old and new values of the property.


**What not to include inside setters:**

Since the PropertyChanged event is raised inside setters, some may find it tempting to also change other related properties inside setters.
For instance, in the Model, when the setter of the property FirstName is invoked, the property FullName must also change accordingly.
As such, besides, raising the PropertyChanged event, it is tempting to also change the property FullName inside the setter of the property FirstName.
However, this is a very bad practice. 

Firstly, a method should only do what it is intended to do, and nothing else. This separation of concern makes the code much easier to maintain and modify.
Suppose that later on, the business logic changes, and the property FullName is no longer changed whenever FirstName changes.
A new developer working on the same project may not know to look into the setter of FirstName to fix this issue.

Secondly, any changes in response to a change of the property should be handled by the event object instead of the setter. 
If all responses to the event are handled solely by the event, it will be much easier to keep track of the responses.
This can be very useful if there are many responses to a single event.
Suppose that later on, apart from FullName, more properties such as NickName, Initials, etc. must also change in response to a change in FirstName.
If these changes are implemented inside the setters of these properties, maintaining and modifying them can be very messy.
In comparison, if these changes are aggregated into a single method subscribed to the event, it will be much easier to keep track of where there are and what they do.













