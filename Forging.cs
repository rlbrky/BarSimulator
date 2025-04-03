using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Forging : MonoBehaviour
{
    #region ForgingSpecifics
    [Header("Elements")] 
    [SerializeField] private Image perfectImage;
    [SerializeField] private Image goodImage;
    [SerializeField] private Image stick;
    [SerializeField] private TextMeshProUGUI remainingHitsText;
    [SerializeField] private RectTransform _backgroundRect; // The background area where the images can move
    

    [Header("Point Values")] 
    [SerializeField] private float perfectValue = 16f; //Perfect hits will grant this much points
    [SerializeField] private float goodValue = 8f; //Good hits will grant this much points
    [SerializeField] private float badValue = 4f;

    
    private float _itemGoodThreshold; //Mid tier item threshold
    private float _itemQualityPts = 0; //Quality tracker
    private int _remainingHitAmount; //Keeps track of hits
    
    private float minX, maxX, minPerfectX, maxPerfectX, minGoodX, maxGoodX; // Boundaries for stick movement
    #endregion
    
    public float itemPerfectThreshold = 100; //Points needed for highest quality item
    public int forgeHitAmount = 10; //Given hits to finish the item.
    public float stickSpeed = 2f; //How fast will the stick move back and forth.
    
    void Start()
    {
        _itemQualityPts = 0;
        _itemGoodThreshold = itemPerfectThreshold * 7 / 10; //70% of perfect threshold.
        _remainingHitAmount = forgeHitAmount;

        float parentWidth = _backgroundRect.rect.width;

        // Get the stick's width
        float stickWidth = stick.rectTransform.rect.width;
        float perfectWidth = perfectImage.rectTransform.rect.width;
        float goodWidth = goodImage.rectTransform.rect.width;

        // Ensure movement from edge to edge with a small 5-pixel padding
        minX = -parentWidth / 2 + (stickWidth / 2) + 5f;
        maxX = parentWidth / 2 - (stickWidth / 2) - 5f;
        
        // Set separate boundaries for hit zones
        minPerfectX = -parentWidth / 2 + (perfectWidth / 2) + 5f;
        maxPerfectX = parentWidth / 2 - (perfectWidth / 2) - 5f;

        minGoodX = -parentWidth / 2 + (goodWidth / 2) + 5f;
        maxGoodX = parentWidth / 2 - (goodWidth / 2) - 5f;
        
        RandomizeHitZones(); // Set initial positions
    }

    void Update()
    {
        MoveStick();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HammerItem();
        }
    }

    private void MoveStick()
    {
        float posX = Mathf.PingPong(Time.time * stickSpeed, maxX - minX) + minX;
        stick.rectTransform.anchoredPosition = new Vector2(posX, stick.rectTransform.anchoredPosition.y);
    }

    private void HammerItem()
    {
        float stickPosX = stick.rectTransform.anchoredPosition.x;
        float perfectStart = perfectImage.rectTransform.anchoredPosition.x - perfectImage.rectTransform.rect.width / 2;
        float perfectEnd = perfectImage.rectTransform.anchoredPosition.x + perfectImage.rectTransform.rect.width / 2;

        float goodStart = goodImage.rectTransform.anchoredPosition.x - goodImage.rectTransform.rect.width / 2;
        float goodEnd = goodImage.rectTransform.anchoredPosition.x + goodImage.rectTransform.rect.width / 2;

        if (stickPosX >= perfectStart && stickPosX <= perfectEnd)
        {
            Debug.Log("Perfect Hit!");
            _itemQualityPts += perfectValue;
        }
        else if (stickPosX >= goodStart && stickPosX <= goodEnd)
        {
            Debug.Log("Good Hit!");
            _itemQualityPts += goodValue;
        }
        else
        {
            Debug.Log("Bad Hit!");
            _itemQualityPts += badValue;
        }

        // Move the hit zones randomly
        RandomizeHitZones();
        
        _remainingHitAmount--;
        remainingHitsText.text = "Hits Left: " + _remainingHitAmount;
        if(_remainingHitAmount <= 0)
        {
            if (_itemQualityPts >= itemPerfectThreshold)
            {
                Debug.Log("Perfect Quality!");
            }
            else if (_itemQualityPts >= _itemGoodThreshold)
            {
                Debug.Log("Good Quality!");
            }
            else
            {
                Debug.Log("Bad Quality!");
            }


            //TODO: Close UI and Script, give item to player.
        }
    }
    
    private void RandomizeHitZones()
    {
        // Generate separate positions for each hit zone
        float newPerfectX = Random.Range(minPerfectX, maxPerfectX);
        float newGoodX = Random.Range(minGoodX, maxGoodX);

        perfectImage.rectTransform.anchoredPosition = new Vector2(newPerfectX, perfectImage.rectTransform.anchoredPosition.y);
        goodImage.rectTransform.anchoredPosition = new Vector2(newGoodX, goodImage.rectTransform.anchoredPosition.y);
    }
}