using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotDef
{
    public float x;
    public float y;
    public bool faceUp = true;
    public string layerName = "Default";
    public int layerID = 0;
    public int id;
    public List<int> hiddenBy = new List<int>();
    public string type = "slot";
    public Vector2 stagger;
}
public class Layout : MonoBehaviour
{
    public PT_XMLReader xmlr;
    public PT_XMLHashtable xml;
    public Vector2 multiplier;

    public List<SlotDef> slotDefs;
    public SlotDef drawPile;
    public SlotDef discardPile;
    public string[] sortingLayerNames = new string[] { "Row0", "Row1", "Row2", "Row3", "Discard", "Draw" };

    public void ReadLayout(string xmlText)
    {
        xmlr = new PT_XMLReader();
        xmlr.Parse(xmlText);
        xml = xmlr.xml["xml"][0];

        multiplier.x = float.Parse(xml["multiplier"][0].att("x"));
        multiplier.y = float.Parse(xml["multiplier"][0].att("y"));

        SlotDef tSD;

        PT_XMLHashList slotsX = xml["slot"];

        for (int i = 0; i <slotsX.Count; i++)
        {
            tSD = new SlotDef();
            if (slotsX[i].HasAtt("type"))
            {
                tSD.type = slotsX[i].att("type");
            }
            else
            {
                tSD.type = "slot";
            }
            tSD.x = float.Parse(slotsX[i].att("x"));
            tSD.y = float.Parse(slotsX[i].att("y"));
            tSD.layerID = int.Parse(slotsX[i].att("layer"));

            tSD.layerName = sortingLayerNames[tSD.layerID];
            
            switch (tSD.type)
            {
                case "slot":
                    tSD.faceUp = (slotsX[i].att("faceup") == "1");
                    tSD.id = int.Parse(slotsX[i].att("id"));
                    if (slotsX[i].HasAtt("hiddenby"))
                    {
                        string[] hiding = slotsX[i].att("hiddenby").Split(',');
                        foreach(string s in hiding)
                        {
                            tSD.hiddenBy.Add(int.Parse(s));
                        }
                    }
                    slotDefs.Add(tSD);
                    break;

                case "drawpile":
                    tSD.stagger.x = float.Parse(slotsX[i].att("xstagger"));
                    drawPile = tSD;
                    break;
                case "discardpile":
                    discardPile = tSD;
                    break;
            }
        }
    }
    public void LayOutPyramid()
    {
        float yOffset = 0.0f; // Adjust this value to control the vertical spacing between rows.

        for (int row = 0; row < numRows; row++)
        {
            int numSlotsInRow = row + 1; // Number of slots in the current row
            float rowWidth = numSlotsInRow * cardWidth;

            for (int slotIndex = 0; slotIndex < numSlotsInRow; slotIndex++)
            {
                SlotDef slot = slotDefs[slotIndex]; // Retrieve the appropriate slot definition
                Vector3 slotPosition = new Vector3(slot.x + (slotIndex * cardWidth), yOffset, 0f);

                // Set the position of the card or game object based on the slot definition
                // Here, you should adjust the position according to the slotPosition.
                // For example, you can move the card's transform to the slotPosition.

                yOffset += cardHeight; // Move to the next row
            }
        }
    }

    // Call this method to lay out cards in a pyramid.
    public void LayOutPyramidCards()
    {
        // Assuming you have defined cardWidth and cardHeight.
        float cardWidth = 1.0f; // Change this to your card's width
        float cardHeight = 1.5f; // Change this to your card's height

        // Assuming numRows is the number of rows in the pyramid.
        int numRows = 5; // Adjust this value based on your pyramid layout.

        LayOutPyramid();
    }

}
