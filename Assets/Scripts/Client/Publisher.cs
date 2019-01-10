namespace Client
{
    public interface Publisher
    {
        void AddSubscriber(Subscriber subscriber);
        void RemoveSubscriber(Subscriber subscriber);
        void Publish();
    }
}