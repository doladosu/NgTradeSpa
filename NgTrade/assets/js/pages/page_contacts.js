var ContactPage = function () {

    return {
        
    	//Basic Map
        initMap: function () {
			var map;
			$(document).ready(function(){
			  map = new GMaps({
				div: '#map',
				lat: 6.437244,
				lng: 3.412392
			  });
			  
			  var marker = map.addMarker({
			      lat: 6.437244,
			      lng: 3.412392,
	            title: 'Company, Inc.'
		       });
			});
        },

        //Panorama Map
        initPanorama: function () {
		    var panorama;
		    $(document).ready(function(){
		      panorama = GMaps.createPanorama({
		        el: '#panorama',
		        lat: 6.437244,
		        lng: 3.412392
		      });
		    });
		}        

    };
}();