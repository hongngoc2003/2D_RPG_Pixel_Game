using UnityEngine;

public class PlayerManager : MonoBehaviour ,ISaveManager {
    public static PlayerManager instance;
    public Player player;
    public int currency;


    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Hủy object mới nếu đã có instance
        } else {
            instance = this; // Gán instance mới
            //DontDestroyOnLoad(gameObject); // Đảm bảo instance tồn tại xuyên scene
        }
    }
    public bool HaveEnoughMoney(int _price) {
        if (_price > currency) {
            Debug.Log("Not enough money");
            return false;
        }

        currency = currency - _price;
        return true;
    }

    public int GetCurrency() => currency;

    public void LoadData(GameData _data) {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data) {
        _data.currency = this.currency;
    }
}
