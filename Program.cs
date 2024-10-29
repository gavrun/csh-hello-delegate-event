namespace csh_hello_delegate_event
{
    // custom event args class to pass additional information with events
    public class CustomEventArgs : EventArgs
    {
        // property to store the message related to the event
        public string Message { get; set; }

        // constructor to initialize the message property
        public CustomEventArgs(string message)
        {
            Message = message;
        }
    }

    // publisher class that raises events for subscribers to listen to
    class Publisher
    {
        // event using eventhandler with custom event argument type
        public event EventHandler<CustomEventArgs> RaiseCustomEvent;

        // method to perform an action and raise the event
        public void DoSomething()
        {
            // raise the event with a new custom event args object
            OnRaiseCustomEvent(new CustomEventArgs("Event triggered"));
        }

        // method to raise the event, allows derived classes to override
        protected virtual void OnRaiseCustomEvent(CustomEventArgs e)
        {
            // local copy of event for thread safety
            EventHandler<CustomEventArgs> raiseEvent = RaiseCustomEvent;

            // check if there are any subscribers
            if (raiseEvent != null)
            {
                // add timestamp to the message
                e.Message += $" at {DateTime.Now}";

                // raise the event with publisher and event data
                raiseEvent(this, e);
            }
        }
    }

    // subscriber class that listens to events from publisher
    class Subscriber
    {
        private readonly string _id; // unique identifier for each subscriber

        // constructor takes id and publisher, subscribes to publisher's event
        public Subscriber(string id, Publisher pub)
        {
            _id = id;
            // subscribe to the raise custom event of publisher
            pub.RaiseCustomEvent += HandleCustomEvent;
        }

        // method that handles the event when raised
        void HandleCustomEvent(object sender, CustomEventArgs e)
        {
            // print subscriber id and the message received
            Console.WriteLine($"{_id} received this message: {e.Message}");
        }
    }

    // entry point
    internal class Program
    {
        static void Main(string[] args)
        {
            // create a publisher instance
            var pub = new Publisher();

            // create two subscriber instances and subscribe them to the publisher's event
            var sub1 = new Subscriber("sub1", pub);
            var sub2 = new Subscriber("sub2", pub);

            // trigger the event by calling the do something method on publisher
            pub.DoSomething();

            // output shows both subscribers reacting to the event
        }
    }
}
