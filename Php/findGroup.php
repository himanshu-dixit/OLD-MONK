<?php
/*
TODO: DELETE ALL THE GROUPS IN WHICH LAST ACTIVITY IS ABOVE 10 MIN OR SOME PERIOD OF TIME. SO THAT WE CAN AVOID FAKE GROUPS
OR THE GROUPS THAT ARE COMPLETE
*/
include('include/database/config.php');
$joined_group  = false;
$final_group = "";
$username = $_POST["username"] ?? "";

$sth = $dbh->prepare("SELECT id FROM groups WHERE user_no<4");
$sth->execute(array());
while($row=$sth->fetch()){
$group_id = $row["id"];
$user_no = $row['user_no'];
$users = $row["users"];
if($user_no==3){
$users.=$username;
}
else{
  $users.=$username.",";
}
$joined_group = true;
/*
Lets increase the number of users in group
*/
$sth = $dbh->prepare("UPDATE groups SET user_no = user_no + 1 WHERE id=:group");
$sth->execute(array(":group"=>$group_id));
$final_group = $group_id;
break;
}
if(!$joined_group){
$sth = $dbh->prepare("INSERT INTO groups (users,user_no) VALUES (:users,1)");
$sth->execute(array(":users"=>$username));
$final_group = $dbh->lastInsertId();
}
echo $final_group;
echo "WOW";
?>
