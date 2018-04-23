function takemethere(timeIndex){
  alert(timeIndex);
}


function PopulateIndex(){
  // var objl = document.getElementById('listIndex');
  for (var i = 0; i < 5; i++) {
    var strl = '<li class="list-group-item"><a href="#" onclick="takemethere('+i+')">Cras justo odio</a></li>';
    $('#listIndex').append(strl)
    // objl.appendChild(strl);
  }
}

$(document).ready(function functionName() {
  PopulateIndex();
});
