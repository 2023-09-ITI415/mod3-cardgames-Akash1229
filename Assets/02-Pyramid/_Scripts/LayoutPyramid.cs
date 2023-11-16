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
public class LayoutPyramid : MonoBehaviour
{
    public PT_XMLReader xmlr;
    public PT_XMLHashtable xml;
    public Vector2 multiplier;

    public List<SlotDef> slotDefs;
    public SlotDef drawPile;
    public SlotDef discardPile;
    public string[] sortingLayerNames = new string[] { "Row0", "Row1", "Row2", "Row3", "Row4", "Row5", "Row6", "Discard", "Draw" };

    public void ReadLayout(string xmlText)
    {
        xmlr = new PT_XMLReader();
        xmlr.Parse(xmlText);
        xml = xmlr.xml["xml"][0];

        multiplier.x = float.Parse(xml["multiplier"][0].att("x"));
        multiplier.y = float.Parse(xml["multiplier"][0].att("y"));

        SlotDef tSD;

        PT_XMLHashList slotsX = xml["slot"];

        int numRows = 7; //number of rows in the pyramid

        for (int i = 0; i <slotsX.Count; i++)
        {
            int numCards = numRows - row; // Number of cards in the current row
            for (int col = 0; col < numCards; col++)
            {
                SlotDef tSD = new SlotDef();
                tSD.type = "slot";
                tSD.x = multiplier.x * (col - numCards / 2f);
                tSD.y = multiplier.y * -row;
                tSD.layerID = row;
                tSD.layerName = sortingLayerNames[tSD.layerID];
                tSD.faceUp = true; // All cards are face up except Discard and Draw

                // Set other slot properties as needed
                tSD.id = row * numRows + col;
                slotDefs.Add(tSD);
            }
        }

        // Set the draw pile and discard pile definitions
        drawPile = new SlotDef();
        drawPile.type = "drawpile";
        drawPile.stagger.x = 0.5f; // Adjust as needed
        discardPile = new SlotDef();
        discardPile.type = "discardpile";
    }
}

