using UnityEngine;
using System.Collections.Generic;

public class LinePooler 
{
    private GameObject _linePrefab;
    private Transform _parent;
    private List<Line> _lines;

    public LinePooler(GameObject linePrefab, Transform parent){
        _linePrefab = linePrefab;
        _parent = parent;

        _lines = new List<Line>();

        GameObject obj = Object.Instantiate(linePrefab, parent);
        obj.SetActive(false);
        _lines.Add(obj.GetComponent<Line>());
    }

    public Line GetPooledLine(){
        foreach(Line line in _lines)
        {
            if(!line.gameObject.activeInHierarchy)
            {
                line.gameObject.SetActive(true);
                return line;
            }
        }

        // If we've reached here, it means all lines in the pool are in use. So, increase pool size.
        GameObject newLine = Object.Instantiate(_linePrefab, _parent);
        Line newLineComponent = newLine.GetComponent<Line>();
        _lines.Add(newLineComponent);
        newLine.SetActive(true);
        return newLineComponent;
    }
}