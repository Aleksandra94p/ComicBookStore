var host = "https://localhost:";
var port = "44379/"; 
var publisherEndpoint = "api/publishers/"
var comicbooksEndpoint = "api/comicbooks/";
var loginEndpoint = "api/authentication/login";
var registerEndpoint = "api/authentication/register";
var formAction = "Create";
var editingId;
var jwt_token;


function validateRegisterForm(username, email, password, confirmPassword) {
	if (username.length === 0) {
		alert("Korisnicko ime ne sme biti prazno.");
		return false;
	} else if (email.length === 0) {
		alert("Polje za email ne sme biti prazno.");
		return false;
	} else if (password.length === 0) {
		alert("Lozinka mora biti uneta.");
		return false;
	} else if (confirmPassword.length === 0) {
		alert("Potvrda lozinke ne sme biti prazna.");
		return false;
	} else if (password !== confirmPassword) {
		alert("Vrednost lozinke i potvrdne lozinke mora da se podudara.");
		return false;
	}
	return true;
}

function registerUser() {
	var username = document.getElementById("usernameRegister").value;
	var email = document.getElementById("emailRegister").value;
	var password = document.getElementById("passwordRegister").value;
	var confirmPassword = document.getElementById("confirmPasswordRegister").value;

	if (validateRegisterForm(username, email, password, confirmPassword)) {
		var url = host + port + registerEndpoint;
		var sendData = { "Username": username, "Email": email, "Password": password };
		fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(sendData) })
			.then((response) => {
				if (response.status === 200) {
					document.getElementById("registerForm").reset();
					console.log("Uspesna registracija!");
					alert("Uspesna registracija!");
					showLogin();
				} else {
					console.log("Greska sa statusnim kodom: " + response.status);
					console.log(response);
					alert("Greska!");
					response.text().then(text => { console.log(text); })
				}
			})
			.catch(error => console.log(error));
	}
	return false;
}

function showLogin() {
	document.getElementById("data").style.display = "block";
	document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "block";
	document.getElementById("registerFormDiv").style.display = "none";
	document.getElementById("loggedIn").style.display = "none";
	document.getElementById("notLoggedIn").style.display = "none";
}


function showRegistration() {
	document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "none";
	document.getElementById("registerFormDiv").style.display = "block";
    document.getElementById("notLoggedIn").style.display = "none";
	
}

function validateLoginForm(username, password) {
	if (username.length === 0) {
		alert("Korisnicko ime ne sme biti prazno.");
		return false;
	} else if (password.length === 0) {
		alert("Lozinka ne sme biti prazna.");
		return false;
	}
	return true;
}

function loginUser() {
	var username = document.getElementById("usernameLogin").value;
	var password = document.getElementById("passwordLogin").value;

	if (validateLoginForm(username, password)) {
		var url = host + port + loginEndpoint;
		var sendData = { "Username": username, "Password": password };
		fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(sendData) })
			.then((response) => {
				if (response.status === 200) {
					console.log("Successful login");
					alert("Uspesna prijava!");
					response.json().then(function (data) {
						console.log(data);
						document.getElementById("info").innerHTML = "Prijavljeni korisnik: <i>" + data.username + "<i/>.";
						document.getElementById("loggedIn").style.display = "block";
						document.getElementById("data").style.display = "block";
						document.getElementById("notLoggedIn").style.display = "none";
						document.getElementById("loginFormDiv").style.display = "none";
						document.getElementById("formDiv").style.display = "none";
					
						jwt_token = data.token;
						loadComicBooks();
					});
				} else {
					console.log("Error occured with code " + response.status);
					console.log(response);
					alert("Desila se greska!");
				}
			})
			.catch(error => console.log(error));
	}
	return false;
}

function loadComicBooks(){
document.getElementById("data").style.display = "block";
document.getElementById("formDiv").style.display = "none";

var requestUrl = host + port + comicbooksEndpoint;
	console.log("URL zahteva: " + requestUrl);
	var headers = {};
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}
	console.log(headers);
	fetch(requestUrl, { headers: headers })
		.then((response) => {
			if (response.status === 200) {
				response.json().then(setComicBooks);
			} else {
				console.log("Error occured with code " + response.status);
				showError();
			}
		})
		.catch(error => console.log(error));
};

function setComicBooks(data){
    var container = document.getElementById("data");
	container.innerHTML = "";

	console.log("Data received from search:");


	console.log(data);

	var div = document.createElement("div");
	var h1 = document.createElement("h1");
	var headingText = document.createTextNode("Stripovi");
	h1.appendChild(headingText);
	div.appendChild(h1);

	var table = document.createElement("table");
	table.className = "table table-hover";

	var header = createHeader();
    header.className = "bg-warning";
	table.append(header);

	var tableBody = document.createElement("tbody");

	for (var i = 0; i < data.length; i++) {
	
		var row = document.createElement("tr");
		
		row.appendChild(createTableCell(data[i].name));
        row.appendChild(createTableCell(data[i].price));
        row.appendChild(createTableCell(data[i].availableQuantity));
        row.appendChild(createTableCell(data[i].publisherName));
		if (jwt_token) {
			row.appendChild(createTableCell(data[i].genre));

			var stringId = data[i].id.toString();

			var buttonEdit = document.createElement("button");
			buttonEdit.name = stringId;
			buttonEdit.addEventListener("click", editComicBook);
			buttonEdit.className = "btn btn-warning";
			var buttonEditText = document.createTextNode("Edit");
			buttonEdit.appendChild(buttonEditText);
			var buttonEditCell = document.createElement("td");
			buttonEditCell.appendChild(buttonEdit);
			row.appendChild(buttonEditCell);

			var buttonDelete = document.createElement("button");
			buttonDelete.name = stringId;
			buttonDelete.addEventListener("click", deleteComicBook);
			buttonDelete.className = "btn btn-danger";
			var buttonDeleteText = document.createTextNode("Delete");
			buttonDelete.appendChild(buttonDeleteText);
			var buttonDeleteCell = document.createElement("td");
			buttonDeleteCell.appendChild(buttonDelete);
			row.appendChild(buttonDeleteCell);
		}
		tableBody.appendChild(row);
	}

	table.appendChild(tableBody);
	div.appendChild(table);

	container.appendChild(div);

}

function showError() {
	var container = document.getElementById("data");
	container.innerHTML = "";

	var div = document.createElement("div");
	var h1 = document.createElement("h1");
	var errorText = document.createTextNode("Greska prilikom preuzimanja podataka!");

	h1.appendChild(errorText);
	div.appendChild(h1);
	container.append(div);
}

function createHeader() {
	var thead = document.createElement("thead");
	var row = document.createElement("tr");
	
	row.appendChild(createTableHeaderCell("Naziv"));
	row.appendChild(createTableHeaderCell("Cena(din.)"));
    row.appendChild(createTableHeaderCell("Dostupna kolicina"));
	row.appendChild(createTableHeaderCell("Izdavac"));

	if (jwt_token) {
		row.appendChild(createTableHeaderCell("Zanr"));
		row.appendChild(createTableHeaderCell("Izmena"));
		row.appendChild(createTableHeaderCell("Brisanje"));
	
	}

	thead.appendChild(row);
	return thead;
}

function createTableHeaderCell(text) {
	var cell = document.createElement("th");
	var cellText = document.createTextNode(text);
	cell.appendChild(cellText);
	return cell;
}

function createTableCell(text) {
	var cell = document.createElement("td");
	var cellText = document.createTextNode(text);
	cell.appendChild(cellText);
	return cell;
}

function deleteComicBook() {
	
	var deleteID = this.name;
	
	var url = host + port + comicbooksEndpoint + deleteID.toString();
	var headers = { 'Content-Type': 'application/json' };
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(url, { method: "DELETE", headers: headers})
		.then((response) => {
			if (response.status === 204) {
				console.log("Successful action");
				refreshTable();
			} else {
				console.log("Error occured with code " + response.status);
				alert("Error occured!");
			}
		})
		.catch(error => console.log(error));
};



function editComicBook() {
    document.getElementById("formDiv").style.display = "block";
    document.getElementById("registerFormDiv").style.display = "none";
    document.getElementById("loginFormDiv").style.display = "none";
    document.getElementById("loggedIn").style.display = "none";
    document.getElementById("notLoggedIn").style.display = "none";
    document.getElementById("data").style.display = "none";
	var editId = this.name;
	

	var url = host + port + comicbooksEndpoint + editId.toString();
	var headers = { };
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(url, { headers: headers})
		.then((response) => {
			if (response.status === 200) {
				console.log("Successful action");
				response.json().then(data => {
					var name = document.getElementById("comicBookName").value = data.name;
					var genre =  document.getElementById("comicBookGenre").value = data.genre;
					var price = document.getElementById("comicBookPrice").value = data.price;
					var quantity = document.getElementById("comicBookAvailableQuantity").value = data.availableQuantity;
                    var publisher = document.getElementById("comicBookPublisher").value = data.publisherId;

					if (!validateInputForm(name, genre, price, quantity)) {
                        return false;
                    }
					editingId = data.id;
					formAction = "Update";
					
				});
			} else {
				formAction = "Create";
				console.log("Error occured with code " + response.status);
				alert("Error occured!");
			}
		})
		.catch(error => console.log(error));
        loadPublishersForDropDown();
};

function validateInputForm(name, genre, price, quantity){
	if (name.length === 0) {
		alert("Naziv mora biti unet.");
		return false;
	} else if(name.length < 3 || name.length > 120) {
		alert("Naziv mora imati izmedju 3 i 120 karaktera.");
		return false;
	} else if (genre.length === 0) {
		alert("Zanr mora biti unet.");
		return false;
	} else if(genre.length < 2 || genre.length > 30) {
		alert("Zanr mora imati izmedju 2 i 30 karaktera.");
		return false;
	} else if (price.length === 0) {
		alert("Cena mora biti uneta.");
		return false;
	} else if (price < 300 || price > 10000) {
		alert("Cena mora biti u intervalu od 300 do 10 000.");
		return false;
	} else if (quantity.length === 0) {
		alert("Kolicina mora biti uneta.");
		return false;
	} else if (quantity < 1 || quantity > 5000 ) {
		alert("Kolicina mora biti u intevalu od 1 do 5000.");
		return false;
	}
	return true;

}

function loadPublishersForDropDown() {
	var requestUrl = host + port + publisherEndpoint;
    

	console.log("URL zahteva: " + requestUrl);

	var headers = {};
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(requestUrl, {headers: headers})
		.then((response) => {
			if (response.status === 200) {
				response.json().then(setPublishersInDropdown);
			} else {
				console.log("Error occured with code " + response.status);
			}
		})
		.catch(error => console.log(error));
};


function setPublishersInDropdown(data) {
	var dropdown = document.getElementById("comicBookPublisher");
	dropdown.innerHTML = "";
    console.log("Podaci:" + data);
	for (var i = 0; i < data.length; i++) {
		var option = document.createElement("option");
		option.value = data[i].id;
		var text = document.createTextNode(data[i].name);
		option.appendChild(text);
		dropdown.appendChild(option);
	}
}


function refreshTable() {
	
	document.getElementById("comicBookName").value = "";
	document.getElementById("comicBookGenre").value = "";
	document.getElementById("comicBookPrice").value = "";
	document.getElementById("comicBookAvailableQuantity").value = "";
	document.getElementById("comicBookPublisher").value = "";
	
	loadComicBooks();
};

function showPasswordRegister(){
    var x = document.getElementById("passwordRegister");
    var y = document.getElementById("confirmPasswordRegister");
    if (x.type === "password" && y.type === "password") {
        x.type = "text";
        y.type = "text";
    } else {
        x.type = "password";
        y.type = "password";
    }
}

function showingPasswordLogin(){ 
    var x = document.getElementById("passwordLogin");
    if (x.type === "password") {
      x.type = "text";
    } else {
      x.type = "password";
    }
  }

  function homePage(){
	jwt_token = undefined;
    document.getElementById("notLoggedIn").style.display = "block";
    document.getElementById("data").style.display = "block";
    document.getElementById("loggedIn").style.display = "none";
    document.getElementById("loginFormDiv").style.display = "none";
    document.getElementById("registerFormDiv").style.display = "none";
    document.getElementById("formDiv").style.display = "none";
	loadComicBooks();
    }


    function submitComicBookForm(){
        var name = document.getElementById("comicBookName").value;
        var genre = document.getElementById("comicBookGenre").value;
        var price = document.getElementById("comicBookPrice").value;
        var quantity = document.getElementById("comicBookAvailableQuantity").value;
        var publisher = document.getElementById("comicBookPublisher").value;
       
		var httpAction;
        var sendData;
        var url;
        
        if (formAction === "Create") {
            httpAction = "POST";
            url = host + port + comicbooksEndpoint;
            sendData = {
                "name": name,
                "genre": genre,
                "price": price,
                "availableQuantity": quantity,
                "publisherId": publisher
            };
        }
        else { 
            httpAction = "PUT";
            url = host + port + comicbooksEndpoint + editingId.toString();
            sendData = {
                "id": editingId,
                "name": name,
                "genre": genre,
                "price": price,
                "availableQuantity": quantity,
                "publisherId": publisher
            };
        }
    
        console.log("Objekat za slanje");
        console.log(sendData);
        var headers = { 'Content-Type': 'application/json' };
        if (jwt_token) {
            headers.Authorization = 'Bearer ' + jwt_token;
        }

		if(!validateInputForm(name, genre, price, quantity)){
			return false;
		}
        fetch(url, { method: httpAction, headers: headers, body: JSON.stringify(sendData) })
            .then((response) => {
                if (response.status === 200 || response.status === 201) {
                    console.log("Successful action");
                    formAction = "Create";
                    refreshTable();
                } else {
                    console.log("Error occured with code " + response.status);
                    alert("Desila se greska!");
                }
            })
            .catch(error => console.log(error));
            document.getElementById("loggedIn").style.display = "block";
		
        return false;
    }

	function back(){
		refreshTable();
		document.getElementById("formDiv").style.display = "none";
		document.getElementById("loggedIn").style.display = "block";
	}

	function validateSearch(min, max){
		if (min.length === 0) {
			alert("Minimalna kolicina mora biti uneta.");
			return false;
		} else if (max.length === 0) {
			alert("Maksimalna kolicina mora biti uneta.");
			return false;
		} else if (min < 0) {
			alert("Minimalna kolicina mora biti veca od 0.");
			return false;
		} else if (max < 0) {
			alert("Maksimalna kolicina mora biti veca od 0.");
			return false;
		} else if( min > max) {
			alert("Maksimalna kolicina mora biti veca od minimalne kolicina.");
			return false;
		} else if (max > 5000 ) {
            alert("Maksimalna kolicna mora biti manja od 5000.");
            return false;
        }
            
		return true;

	}

	function search(){

		var httpAction;
		var sendData;
		var url;
	
		var min = document.getElementById("najmanje").value;
		var max = document.getElementById("najvise").value;


		if (!validateSearch(min, max)) {
			return false;
		}
	
		url = host + port + "api/pretraga?min=" + min + "&max="+ max;
	
		console.log("URL zahteva: " + url);
	
		var headers = {};
		if (jwt_token) {
			headers.Authorization = 'Bearer ' + jwt_token;
		}

		var headers = { 'Content-Type': 'application/json', 'charset': 'utf-8' };
		if (jwt_token) {
			headers.Authorization = 'Bearer ' + jwt_token;
		}
	
		var body = JSON.stringify({ min: min, max: max });
	
	
		fetch(url, { method: "POST", headers: headers, body: body })
			.then((response) => {
				if (response.status === 200) {
					console.log(response);
					response.json().then(setComicBooks);
				} else {
					console.log("Error occured with code " + response.status);
					showError();
				}
			})
			.catch(error => console.log(error));
		return false;
	};
	
	