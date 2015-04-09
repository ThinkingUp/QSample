var appp = angular.module('mapping', []);
appp.directive('ngSparkline', function () {
    return {
        restrict: 'A',
        transclude: true,
        require: '^ngModel',
        scope: {
            ngModel: '=',
            ngLocation: '=',
            ngInit: '&'
        },
        templateUrl: 'map_template.html',
        controller: ['$scope', function ($scope) {
            $scope.initialize = function (element, locations) {
                var mapOptions = {
                    center: { lat: locations[0].lat, lng: locations[0].lng },
                    zoom: 13
                };
                var map = new google.maps.Map(element,
                    mapOptions);
                var myLatlng = new google.maps.LatLng(locations[0].lat, locations[0].lng);
                // To add the marker to the map, use the 'map' property
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: "Hello World!"
                });
                var trafficLayer = new google.maps.TrafficLayer();
                trafficLayer.setMap(map);
                var infowindow = new google.maps.InfoWindow({
                    content: "<div>anij</div>"
                });
                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.open(map, marker);
                });
            }
        }],
        link: function (scope, iElement, iAttrs, ctrl) {
            console.log(scope.ngLocation);
            scope.initialize(document.getElementById('map-canvas'), scope.ngLocation);
        }
    }
});