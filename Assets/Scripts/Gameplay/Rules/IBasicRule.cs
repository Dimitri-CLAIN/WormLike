
interface IBasicRule<T>
{
    protected T DefaultsValue { get; }
    protected T Value { get; }

    void UpdateValue(T newValue);
    void SetAsDefaultValue();
}