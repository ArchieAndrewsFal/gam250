<?php

//Post all the data from the game.
$newMovementData = $_POST["newMovementData"];
$id = $_POST["idToEdit"];

//Store all the login information for the sql.
$servername = "localhost";
$username = "root";
$password = "FalmouthGAM250";
$dbname = "MovementData";

//Create a new connection to the sql server
$newConnection = mysql_connect($servername, $username, $password);

//If no connection run an error
if(!$newConnection)
{
die('Error Connection: ' . mysql_error());
}

//Select the correct table
mysql_select_db($dbname);

//Update existing entry using the id sent over
$sql = "UPDATE $dbname SET MovementData='$newMovementData' WHERE SessionID='$id'";

mysql_query($sql);	//Send the data off
?>









