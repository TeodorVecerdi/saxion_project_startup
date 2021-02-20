public interface IBuilder {
    void Build(UserModel userModel);
    void Cleanup();
}