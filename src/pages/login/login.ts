import { HelperServiceProvider } from './../../providers/helper-service/helper-service';
import { Storage } from '@ionic/storage';
import { Component, ViewChild, OnInit } from "@angular/core";
import { NavController, AlertController, ToastController, MenuController, Events } from "ionic-angular";
import { HomePage } from "../home/home";
import { AuthServiceProvider } from "../../providers/auth-service/auth-service";
import { NgForm } from '@angular/forms';
@Component({
  selector: 'page-login',
  templateUrl: 'login.html'
})
export class LoginPage implements OnInit {
  url: string;
  data: string;
  username: string = "";
  password: string = "";
  remember: number;
  showOptions: boolean = true;
  TOKEN_KEY: string = 'token';
  REMEMBER_KEY: string = 'remember';
  USERNAME_KEY: string = 'username';
  PASSWORD_KEY: string = 'password';
  networkAddress: string = 'localhost';
  message_connexion: string = '';
  connexionBool: boolean = true;
  @ViewChild('rememberMe') rememberMeToggle;
  constructor(public nav: NavController,
    public addrCtrl: AlertController,
    public menu: MenuController,
    public toastCtrl: ToastController,
    public authService: AuthServiceProvider,
    private storage: Storage,
    private helperService: HelperServiceProvider,
    public events : Events
  ) {
    this.menu.swipeEnable(false);
  }

  async  OnSignin(form: NgForm) {
    this.username = form.value.username;
    this.password = form.value.password;
    await this.authService.getAuthentification(this.username, this.password).subscribe(data => {

      if (data.access_token != null) {
        this.authService.setToken(data.access_token);
         this.getAccountDetail();
        this.rememberMeSave(this.username, this.password);
        this.nav.setRoot(HomePage);
      }
    }, (error) => {
      if (error == "invalid_grant") {       
        this.helperService.showNotifError("l'identifiant ou le mot de passe est incorrect");
      }
      else {
        this.helperService.showNotifError("l'application n'a pas pu se connecter au seveur");
      }
    });
  }
 public async  getAccountDetail() {
   await this.authService.getAccountDetail().subscribe((data) => {
      console.log("Account detail", data);
      this.helperService.USERNAME = data.UserId;      
      this.events.publish('user:username', this.helperService.USERNAME );
    }, (error) => {
      this.helperService.showNotifError(error);
    })
  }
  // login and go to home page
  // remember username and password
  rememberMeSave(username: string, password: string) {
    if (this.rememberMeToggle.value) {
      this.storage.set(this.REMEMBER_KEY, 1);
      this.storage.set(this.USERNAME_KEY, username);
      this.storage.set(this.PASSWORD_KEY, password);
    }
    else {
      this.storage.set(this.REMEMBER_KEY, 0);
      this.storage.set(this.USERNAME_KEY, " ");
      this.storage.set(this.PASSWORD_KEY, " ");
    }
  }
  testConnexion() {
    console.log("enter in the testconnexion method");

    this.authService.testConnexion().subscribe(data => {
      if (data == "ok") {
        this.message_connexion = "vous etes bien connectee";
        this.connexionBool = true;
      }
    }, (error) => {
      this.message_connexion = "verifier que vous avez entrez la bonne address serveur";
      this.connexionBool = false;
      console.log(error);
    })
  }
  setAddressNetwork() {
    this.networkAddress = this.helperService.networkAddress;
    let addresseInput = this.addrCtrl.create({
      title: 'Adresse du serveur',
      message: "Entrez l'adresse de votre serveur.",
      inputs: [
        {
          name: 'addresse',
          placeholder: 'address',
          type: 'text',
          value: this.networkAddress
        },
      ],
      buttons: [
        {
          text: 'Annuler',
          handler: data => {
            console.log('Cancel clicked');
          }
        },
        {
          text: 'Save',
          handler: data => {
            this.helperService.saveNetworkAddress(data.addresse);
            this.helperService.showNotifSuccess("L'adresse a bien été mise à jour")
          }
        }
      ]
    });
    addresseInput.present();
  }

  loadLoginData() {
    this.storage.get(this.REMEMBER_KEY).then((val: number) => {
      this.remember = val;
      if (this.remember == 1) {
        this.storage.get(this.USERNAME_KEY).then((val: string) => {
          this.username = val;
        }).catch((error) => error);

        this.storage.get(this.PASSWORD_KEY).then((val: string) => {
          this.password = val;
        }).catch((error) => error);
      }
    }).catch((error) => error);
  }
  ngOnInit(): void {
    this.loadLoginData();

  }
}
