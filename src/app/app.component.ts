import { DashboardServiceProvider } from '../providers/dashboard-service/dashboard-service';
import { DashboardPage } from '../pages/dashboard/dashboard';
import { HelperServiceProvider } from '../providers/helper-service/helper-service';
import { EncaissementsPage } from '../pages/encaissements/encaissements';
import { Component, ViewChild } from "@angular/core";
import { Platform, Nav, AlertController, App, Events } from "ionic-angular";
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { Keyboard } from '@ionic-native/keyboard';
import { HomePage } from "../pages/home/home";
import { LoginPage } from "../pages/login/login";
export interface MenuItem {
  title: string;
  component: any;
  icon: string;
}
@Component({
  templateUrl: 'app.html'
})
export class MyApp {
  @ViewChild(Nav) nav: Nav;
  rootPage: any = LoginPage;
  appMenuItems: Array<MenuItem>;
  username: string = '';
  role: string = '';
  constructor(
    public platform: Platform,
    public statusBar: StatusBar,
    public splashScreen: SplashScreen,
    public keyboard: Keyboard,
    public helperService: HelperServiceProvider,
    public alertCtrl: AlertController,
    public dashBoardService: DashboardServiceProvider,
    private app: App,
    public events : Events

  ) {
    this.initializeApp();
    this.appMenuItems = [
      { title: 'Accueil', component: HomePage, icon: 'home' },
      { title: 'Journal', component: EncaissementsPage, icon: 'logo-usd' },
      { title: 'Analyse statistique', component: DashboardPage, icon: 'stats' },

    ];
    this.events.subscribe('user:username',(username) => {
      console.log("we recieved username");      
      this.username = username;
    });
  }

  async initializeApp() {
    await this.platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      //*** Control Splash Screen
      //this.splashScreen.show();
      // this.splashScreen.hide();
      this.helperService.getNetworkAdress().then(() => {
        
      }
      );
      //*** Control Status Bar
      this.statusBar.styleDefault();
      this.statusBar.overlaysWebView(false);
      //*** Control Keyboard
      //this.keyboard.disableScroll(true);
    });


    this.platform.registerBackButtonAction(() => {

      // Catches the active view

      let nav = this.app.getActiveNavs()[0];

      let activeView = nav.getActive();

      // Checks if can go back before show up the alert
     
        if (activeView.name === 'HomePage' || activeView.name === 'LoginPage') {
          if (nav.canGoBack()) {
            nav.pop();
          } else {
            const alert = this.alertCtrl.create({
              title: 'App',
              message: 'Voulez vous quiter l\'application?',
              buttons: [{
                text: 'non',
                role: 'cancel',
                handler: () => {
                  this.nav.setRoot('HomePage');
                }
              }, {
                text: 'oui',
                handler: () => {
                  this.logout();
                  this.platform.exitApp();
                }
              }]
            });
            alert.present();
          }
        } else {
          nav.pop();
        }
    });
  }
  openPage(page) {
    // Reset the content nav to have just this page
    // we wouldn't want the back button to show in this scenario
  
     
    this.nav.setRoot(page.component);

  }

  logout() {
    this.nav.setRoot(LoginPage);
  }

}
