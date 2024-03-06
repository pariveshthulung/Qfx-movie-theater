$(".date-frm-db").click(function() {
    var date = $(this).val();
    var showID = @Model.Show.ID;
    var url = "/Public/Public/GetTime?date="+ date +"&showID=" + showID;
    fetch(url)
    .then((res)=>{
        return res.json()
    }).then((data)=>{ 
        console.log(data)
        let placeholder = $("timeDB");
                let out = "";
        data.data.map((value)=>{
            console.log(value);
            time1 = value.time;
            console.log(time1);
            out += `
                <span class="timeToShine border rounded-3 p-2 text-dark">${value.time.slice(11,16)} <input class="timeToShinw" value="${value.id}" hidden></span>                                 
                 &nbsp; &nbsp;    
            `;
            
        });
        document.getElementById("timeDB").innerHTML = out;
        
    }).catch((error)=>{
        console.log(error)});
});



var timelist = document.getElementsByClassName("helloMan");
var countTime = timelist.length;
for(var i = 0; i<countTime; i++){
    timelist[i].addEventListener("click",myfunction);
}
function myfunction(){

alert("hello");
} 