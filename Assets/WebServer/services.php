<?php

class GameLevel {

	var $id = "";
	var $gamekey = "";
	var $area = "";
	var $autor = "";
	var $title = "";
	var $level = 1;
	var $argument = "";
	var $password = "";
	var $live = "";
	var $created = "";
	
	function updateTo( $row ) {
		$this->id = "".$row["id"]; // level
		$this->gamekey = $row["gamekey"];
		$this->area = $row["area"];
		$this->autor = $row["autor"];
		$this->title = $row["title"];
		$this->level = $row["level"];
		$this->argument = $row["argument"];
		$this->password = $row["password"];
		$this->live = $row["live"];
		$this->created = $row["created"];
	}
}

 // services
 $service = "";
 if (isset($_REQUEST["service"])) {
 	$iservice = "".$_REQUEST["service"];
 	if ($iservice=="getareas") $service = "getareas";
 	if ($iservice=="getareaautors") $service = "getareaautors";
 	if ($iservice=="get") $service = "get";
 	if ($iservice=="set") $service = "set";
 }
 
  // echo("service: ".$service);
  
  // exit();
 
 // areas
 $gamekey = "swissmercenaries";
 $area = "";
 $autor = "";
 $title = "";
 $level = -1;
 $ipassword = "";
 $argument = "";
 $level = "-1";
 if ($service!="") {
 	if (isset($_REQUEST["gamekey"])) $igamekey = "".$_REQUEST["gamekey"];
 	$gamekey = urlencode( $igamekey );
 	$ititle = "".$_REQUEST["title"];
 	$area = urlencode($iarea);
 	$iarea = "".$_REQUEST["area"];
 	$area = urlencode($iarea);
 	$iautor = "".$_REQUEST["autor"];
 	$autor = urlencode($iautor);
 	$ipassword = urlencode("".$_REQUEST["password"]);
 	$ilevel = "".$_REQUEST["level"];
 	// echo($ilevel);
	$level = urlencode($ilevel);
 	 	
 }
 if ($service=="set") {
 	$iargument = "".$_REQUEST["argument"];
 	// check .. 
 	$argument = $iargument;
 }
 
 // key // overwritting
 $gamekey = "swissmercenaries";
 
 if ($service=="") {
	 echo("#reislauefer-rest-service\n");
	 echo("- getareas http://www.swissmercenariesgame.com/services.php?service=getareas\n");
	 echo("- getareaautors http://www.swissmercenariesgame.com/services.php?service=getareaautors&area=classic\n");
	 
	 echo("- get http://www.swissmercenariesgame.com/services.php?service=get&area=classic&autor=t00cg&level=1\n");
	 echo("- set http://www.swissmercenariesgame.com/services.php?service=set&area=classic&autor=t00cg&level=2&argument=aksdfjalsdf\n");
 }
 
 if ($service!="") {
 
 	$servername = "localhost";
	$username = "swissme_process";
	$password = "--------";
	$dbname = "swissme_processwire";

	// Create connection
	$conn = new mysqli($servername, $username, $password, $dbname);
	// Check connection
	if ($conn->connect_error) {
		die("Connection failed: " . $conn->connect_error);
	} 

	$sql = "SELECT * FROM gamelevel WHERE gamekey ='".$gamekey."'";

	// getareas
	if ($service=="getareas") {
		$sql = "SELECT distinct(area) FROM gamelevel WHERE gamekey ='".$gamekey."' order by area";
		$result = $conn->query($sql);
		if ($result->num_rows > 0) {
			// output data of each row
			$arr =  array();
			while($row = $result->fetch_assoc()) {
		
				// echo "id: " . $row["id"]. " - area: " . $row["area"]. "- autor: " . $row["autor"]. " / " . $row["level"]." " . $row["argument"]. "<br>";
				// json_decode
				$obj = new GameLevel();
				$obj->updateTo($row);
				$arr[count($arr)] = $obj;
			
			}
			$strJSON = json_encode( $arr );
			echo($strJSON);
		} else {
			echo "0 results";
		}
		$conn->close();
	}
	
	if ($service=="getareaautors") {
		$sql = "SELECT distinct(autor) FROM gamelevel WHERE gamekey ='".$gamekey."' AND area='".$area."' ";
		// echo($sql);
		$result = $conn->query($sql);
		if ($result->num_rows > 0) {
			// output data of each row
			$arr =  array();
			while($row = $result->fetch_assoc()) {
		
				// echo "id: " . $row["id"]. " - area: " . $row["area"]. "- autor: " . $row["autor"]. " / " . $row["level"]." " . $row["argument"]. "<br>";
				// json_decode
				$obj = new GameLevel();
				$obj->updateTo($row);
				$arr[count($arr)] = $obj;
			
			}
			$strJSON = json_encode( $arr );
			echo($strJSON);
		} else {
			echo "0 results";
		}
		$conn->close();
	}	

	if ($service=="get") {
		$sql = "SELECT * FROM gamelevel WHERE gamekey ='".$gamekey."' AND area='".$area." AND level = '".$level."' ";
		// echo($sql);
		$result = $conn->query($sql);
		if ($result->num_rows > 0) {
			// output data of each row
			$arr =  array();
			while($row = $result->fetch_assoc()) { 
		
				// echo "id: " . $row["id"]. " - area: " . $row["area"]. "- autor: " . $row["autor"]. " / " . $row["level"]." " . $row["argument"]. "<br>";
				// json_decode
				$obj = new GameLevel();
				$obj->updateTo($row);
				$arr[count($arr)] = $obj;
				echo($obj->argument);
				break;
			}
			$strJSON = json_encode( $arr );
			// echo($strJSON);
		} else {
			echo "[]";
		}
		$conn->close();
		exit();
	} 
	
	if ($service=="set") {
		// replace ...
		$sql = "insert into gamelevel (gamekey,area,autor,level,argument) values ('".$gamekey."','".$area."','".$autor."','".$level."','".$argument."') ";
		echo("SQL: "$sql);   
		echo("[{\"result\":\"done\"}]");
		echo($argument);
		$conn->query($sql);
	 	$conn->close();   
		exit();
	}	

	
 }

?>