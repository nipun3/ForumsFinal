app.controller("category", ['$scope', 'categoryService',
function category($scope, categoryService) {
    debugger;
    $scope.status;
    $scope.CategoriesList;
    $scope.mySelections = [];
    $scope.currentCategoryDetails = {};
    $scope.gridOptions = {
        data: 'CategoriesList',
        selectedItems: $scope.mySelections,
        multiSelect: false
    };
    $scope.init = function () {
        $scope.getCategories();
    }
    $scope.getCategories = function () {
        categoryService.getcategories()
            .then(function (response) {
                $scope.CategoriesList = response.data;
            }, function (error) {
                $scope.status = 'Unable to load Category data: ' + error.message;
            });
    }
    //Open Add Category Form
    $scope.openAddCategoryForm = function () {
        categoryService.showAddCategoryForm('add-edit-main');
        $scope.counties = {};
        $scope.currentCategoryDetails = {};
        $scope.$apply();
    };
    //Populate the data of Selected Category
    $scope.openUpdateCategoryForm = function (e) {
        $scope.currentCategoryDetails = $scope.mySelections //selected row data
        $scope.selectedCategoryId = $scope.currentCategoryDetails.CategoryId;
        categoryService.showEditCategoryForm('add-edit-main');
        $(".alert").hide();
        $(".modal-header.alert.alert-success").show();
        $scope.isEditCategory = true;
        $scope.$apply();

        $scope.assignCounty('divEditZipCode');
    };
    $scope.insertCategory = function () {
        categoryService.insertcategory($scope.currentCategoryDetails).then(function (response) {

        }, function (error) {
            $scope.status = 'Unable to load Category data: ' + error.message;
        });
    }
    $scope.resetPage = function () {
        categoryService.resetPage('add-edit-main');
    }

}]);
