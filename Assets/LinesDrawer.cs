using UnityEngine;

public class LinesDrawer : MonoBehaviour {

	public GameObject linePrefab;
	
	// int cantDrawOverLayerIndex;

	[Space ( 30f )]
	public Gradient lineColor;
	public float linePointsMinDistance;
	public float lineWidth;

	private Line _currentLine;
	private LinePooler _linePooler;
	[SerializeField] Camera cam;


	void Start ( ) {
		cam = Camera.main;
		_linePooler = new LinePooler(linePrefab, this.transform);
	}

	void Update ( ) {
		if ( Input.GetMouseButtonDown ( 0 ) )
			BeginDraw ( );

		if ( _currentLine != null )
			Draw ( );

		if ( Input.GetMouseButtonUp ( 0 ) )
			EndDraw ( );
	}

	// Begin Draw ----------------------------------------------
	void BeginDraw ( ) {
		// currentLine = Instantiate ( linePrefab, this.transform ).GetComponent <Line> ( );
		_currentLine = _linePooler.GetPooledLine();
		if (_currentLine == null)
		{
			Debug.Log("no line available in the pool.");
			return; // If no line available in the pool.
		}
		
		//Set line properties
		_currentLine.UsePhysics ( false );
		_currentLine.SetLineColor ( lineColor );
		_currentLine.SetPointsMinDistance ( linePointsMinDistance );
		_currentLine.SetLineWidth ( lineWidth );

	}
	// Draw ----------------------------------------------------
	void Draw ( ) {
		Vector2 mousePosition = cam.ScreenToWorldPoint ( Input.mousePosition );

		// //Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
		// RaycastHit2D hit = Physics2D.CircleCast ( mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer );

		// if ( hit )
		// 	EndDraw ( );
		// else
			_currentLine.AddPoint ( mousePosition );
	}
	// End Draw ------------------------------------------------
	void EndDraw ( ) {
		if ( _currentLine != null ) 
		{
			if ( _currentLine.pointsCount < 2 ) 
			{
				//If line has one point
				StartCoroutine(_currentLine.ResetLine(0));
			} 
			else
			{
				//Activate Physics on the line
				// Set it to 'true' if you want line to fall down after drawing it.
				_currentLine.UsePhysics ( false );
				StartCoroutine(_currentLine.ResetLine(5));
				_currentLine = null;
			}
		}
	}
}
