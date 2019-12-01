$(document).ready(function () {

  urlParams = new URLSearchParams(window.location.search);
params = {};

urlParams.forEach((p, key) => {
  params[key] = p;
  });


var urlProj = 'goods.json';
    var getProjects = $.ajax({
        url: urlProj,
        type: 'GET',
        dataType: 'json',
        success:function(data) {
          var obj=JSON.stringify(data);
             
         obj=JSON.parse(obj);
          //console.log(obj.data);
        


            for (var i=0; i<=obj.data.length-1; i++) {
                var project = obj.data[i];
      //console.log(project.price)
       var url=project.id;
            var priceold=project.price;;
                      var pricemain=project.finalPrice;
                      var title=project.title;
                      var image =project.imageUrl;
                      var cat=project.category;
                
 
  
  
$(".category").append('<a class="dropdown-item" href="#">'+cat+'</a>');


console.log(project.category);
                     

                      $( ".cards" ).append( '<a href="/black_friday_card.html?id='+url+'">  <div class="card" style="width: 18rem; "> <div class=card-img><img src="'+image+'" class="card-img-top" alt="... "> </div><div class="card-body"> <h5 class="card-title">'+title+'</h5> <p class="card-text">Brand </p> </div> <div class="pr"> <p class="old-p"> '+priceold+'</p> <div class="new-p"> <div class="box-main-price"> <a href="#" id="main_price" class="main_price_item">'+pricemain+'Руб</a></div><p class="prr">купить</p></div> </div> </div> </div> </div> </a>  ');
            }
 


            $(".more").append('<button>Показать еще</button>');
}
            
        
    });





var urlProj = 'card.json';
    var getProjects = $.ajax({
        url: urlProj,
        type: 'GET',
        dataType: 'json',
        success:function(data) {
           

      var obj=JSON.stringify(data);
             
         obj=JSON.parse(obj);
        
         $('h4#card').text(obj.title);
         $('.old_price ').append('<span class="d24"></span>'+obj.price+'руб');
         $('#main_price ').text(obj.finalPrice+'руб');
         $('.img_box ').append('<img src="'+obj.imageUrl+'" class="img_card_item" alt="img">');

         var vigod=parseInt(obj.price)-parseInt(obj.finalPrice);
        $('#discount_price').text('выгода'+vigod+'рублей!') 
         //console.log(vigod);


 
         //console.log(obj.parameters);
         for (var i=0; i<=obj.parameters.length-1; i++) {
                var param = obj.parameters[i];
            // console.log(param.title);
             // console.log(param.value);
              $('#body_params').append('  <div class="list-group-item list-group-item-action">  <div class="row"> <div class="title col-md-4 border-right">'+param.title+':</div> <div class="col-md-8">'+param.value+'</div> </div>   </div> </div>')
           
              
              
          
          

            }


                
                          



    }
       });
     

     



});