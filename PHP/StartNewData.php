<?php

//Post all the data from the game.
$movementData = $_POST["movementData"];
$date = $_POST["date"];
$sessionTag = $_POST["sessionTag"];

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

//Create new entry into the sql.
$sql = "INSERT INTO MovementData (MovementData, SessionTag, Date) VALUES ('$movementData', '$sessionTag ', '$date')";

mysql_query($sql);	//Send the data off
$returnID = mysql_insert_id();	//Grab the auto increment id so we can keep updating the same row with new data 
echo $returnID; //Print the id so we can grab in it C#
?>