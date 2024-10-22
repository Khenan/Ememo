public class PhotonStream : Singleton<PhotonStream>
{
    public void StreamBoolProperty(Photon.Pun.PhotonStream stream, ref bool _property)
    {
        if (stream.IsWriting) stream.SendNext(_property);
        else _property = (bool)stream.ReceiveNext();
    }
    public void StreamIntProperty(Photon.Pun.PhotonStream stream, ref int _property)
    {
        if (stream.IsWriting) stream.SendNext(_property);
        else _property = (int)stream.ReceiveNext();
    }
    public void StreamStringProperty(Photon.Pun.PhotonStream stream, ref string _property)
    {
        if (stream.IsWriting) stream.SendNext(_property);
        else _property = (string)stream.ReceiveNext();
    }
    public void StreamCharacterProperty(Photon.Pun.PhotonStream stream, ref Character _property)
    {
        if (stream.IsWriting) stream.SendNext(_property);
        else _property = (Character)stream.ReceiveNext();
    }
}