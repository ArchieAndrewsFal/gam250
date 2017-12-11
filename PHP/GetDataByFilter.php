<?php

//Post all the data from the game.
$filterType = $_POST["filterType"];
$filter1 = $_POST["filter1"];
$filter2 = $_POST["filter2"];

//Store all the login information for the sql.
$servername = "localhost";
$username = "root";
$password = "FalmouthGAM250";
$dbname = "MovementData";

//Filters
$limit = "limit";
$id = "sessionId";
$sessionRange = "sessionRange";
$range = "range";
$date = "date";
$dateRange = "dateRange";
$sessionTag = "sessionTag";

$tag;

//Create a new connection to the sql server
$newConnection = mysql_connect($servername, $username, $password);

//If no connection run an error
if(!$newConnection)
{
	die('Error' . mysql_error());
}

//Select the correct table
mysql_select_db($dbname);

//IF the filter is limit then run the limit SQL code
if($limit == $filterType)
{
	$sql = "SELECT * FROM $dbname Limit $filter1"; //Select the first x amount of data defined by the limit
	$tag = '[Sessions]'; //Tag the data so we know what we are getting on the other side
}

if($sessionRange == $filterType)
{
	$sql = "SELECT * FROM $dbname WHERE SessionID BETWEEN $filter1 AND $filter2"; //Select the data between the two filters
	$tag = '[Sessions]';
}

if($date == $filterType)
{
	$sql = "SELECT * FROM $dbname WHERE Date=DATE('$filter1')"; //Select the data where the data is the same as filter1 
	$tag = '[Sessions]';
}

if($dateRange == $filterType)
{
	$sql = "SELECT * FROM $dbname WHERE Date BETWEEN DATE('$filter1') AND DATE('$filter2')"; //Select the data where the date is between filter1 and filter2
	$tag = '[Sessions]';
}

if($sessionTag == $filterType)
{
	$sql = "SELECT * FROM $dbname WHERE SessionTag='$filter1'";
	$tag = '[Sessions]';
}

if($id == $filterType)
{
	$sql = "SELECT * FROM $dbname WHERE SessionID=$filter1";
	$tag = '[MovementData]';
}

if($range == $filterType)
{
	$sql = "SELECT * FROM $dbname WHERE SessionID BETWEEN $filter1 AND $filter2";
	$tag = '[MovementData]';
}

$result = mysql_query($sql, $newConnection);	//Get the data from the server

if (!$result) 
{
    echo 'Error' . mysql_error();
    exit;
}

echo $tag;
while($row = mysql_fetch_assoc($result))
{
	echo '#'; //print a symbol so we can seperate the numbers	//echo this at the begining so we don't end up with a blank string when we break this up in Unity
	if($limit == $filterType) echo $row['SessionID']; //Draw the session id for filters that return a session
	if($sessionRange == $filterType) echo $row['SessionID'];
	if($date == $filterType) echo $row['SessionID'];
	if($dateRange == $filterType) echo $row['SessionID'];
	if($sessionTag == $filterType) echo $row['SessionID'];

	if($id == $filterType) echo $row['MovementData']; //Draw the movement data for filters that request the movement data
	if($range == $filterType) echo $row['MovementData'];
}
?>