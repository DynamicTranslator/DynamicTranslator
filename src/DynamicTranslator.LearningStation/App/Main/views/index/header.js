(function () {
    var controllerId = "headerController";
    angular.module("app").controller(controllerId, ["$rootScope", function ($scope) {
        var vm = this;
        vm.play = "play";
    }
    ]);
})();