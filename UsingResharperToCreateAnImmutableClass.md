# Using ReSharper Tool to Create an Immutable Class

If you have the JetBrains ReSharper tool for Visual Studio it is a trivial task to create a whole immutable class from a single line constructing the object before a class representing the object even exists in your solution.

## Process
Given the following line, and assuming that no class called `MessageEvent` already exists anywhere in your Visual Studio project solution.

    var @event = new MessageEvent(sender, CreateMessageTextBox.Text);

### Create a new class from the constructor call
    
Click on the `MessageEvent` constructor word. Observe the red reSharper light bulb in the left margin and mouse-over it. You will see a tool tip "Create type 'MessageEvent' (Alt + Enter)". Click the "Create type 'MessageEvent'" option and this will create a new class of this type in the current file. Select "class" or "struct", then tab through the constructor argument types and names, changing as required. Clear out the `throw new System.NotImplementedException();` from the constructor.

    internal class MessageEvent
    {
        public MessageEvent(object sender, string text)
        {
                
        }
    }

### Move the class to its own file
Click on the class type name, in this case `MessageEvent`, observer the reSharpoer hammer in the left margin and use the "Move to 'MessageEvent.cs'" to create the class in its own file. Using Visual Studio Solution Explorer, move the `MessageEvent.cs` to your preferred location.

### Correct the class namespace
Click on the class namespace and use the ReSharper light bulb in the left margin to adjust the namespace to suit the location or correct it manually.

### Introduce Fields for the Constructor Arguments
Click on each field and use the ReSharper light bulb in the left margin to "Introduce and initialize field"

    internal class MessageEvent
    {
        private readonly object _sender;
        private readonly string _text;

        public MessageEvent(object sender, string text)
        {
            _sender = sender;
            _text = text;
        }
    }

### Encapsulate field
Click on the field name and use the ReSharper light bulb in the left margin to "Encapsulate Field". Select the appropriate options, scope usage, etc, in this case I will just use "Read Usages".

    internal class MessageEvent
    {
        private readonly object _sender;
        private readonly string _text;

        public MessageEvent(object sender, string text)
        {
            _sender = sender;
            _text = text;
        }

        public object Sender
        {
            get { return _sender; }
        }

        public string Text
        {
            get { return _text; }
        }
    }

And there you have it. An immutable class created with ease from a single constructor statement using ReSharper. Enjoy your new found productivity!

### ReSharper Download
You can download a free 30 day trial of JetBrains ReSharper [here](https://www.jetbrains.com/resharper/)
