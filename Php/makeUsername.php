<?php
include('include/database/config.php');
$username = $_POST["username"] ?? "anonymous";
$final = "";
function makeUsername($username){
  global $final;
  global $dbh;
if(!isset($username) || empty($username) ){
  return array(false);
}
$prefix = md5(uniqid(rand(), true));
$username.="_".$prefix;
$sth = $dbh->prepare("SELECT username FROM users WHERE username=:username");
$sth->execute(array(":username"=>$username));
while($row=$sth->fetch()){
$final_username = $row['username'];
}
if(isset($final_username)){
  return array(false);
}
$final = $username;
$sth = $dbh->prepare("INSERT INTO users (username,available) VALUES (:username,false)");
$sth->execute(array(":username"=>$username));
/*
INSERT TO DATABASE WITH USERNAME
*/
return array(true,$username); // Lets finally return the value
}
while(makeUsername($username)[0] === false){

}
$send_username_var = $final;
echo $send_username_var;
?>
