import { Storage } from '@ionic/storage';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
import { ToastController } from 'ionic-angular';
@Injectable()
export class HelperServiceProvider {
    NETWORK_ADDRESS_KEY: string = 'network_address';
    DATE_STATISTIC_KEY: string = 'Date_Statistic';
    USERNAME: string = ' ';
    ROLE: string = ' ';
    networkAddress: string;
    datesStatistic = new Array();
    public constructor(
        private storage: Storage,
        public toastCtrl: ToastController) {
        this.getDatesStatistic();
    }
    async getNetworkAdress() {
        await this.storage.get(this.NETWORK_ADDRESS_KEY).then((networkAddresse) => {
            this.networkAddress = networkAddresse;
            if (this.networkAddress == null) {
                this.saveNetworkAddress("http://localhost/");
            }
        }, (error) => {
            this.showNotifError("Network Address :" + error)
        });
        return this.networkAddress;
    }
    getDatesStatistic() {
        this.storage.get(this.DATE_STATISTIC_KEY).then((dates) => {
            if (dates != null) {
                this.datesStatistic = dates;
            }
        }, (error) => {
            this.showNotifError(" statistique :" + error)

        });
    }
    async deleteDateStat(dateDebut, dateFin) {
        var position = this.datesStatistic.findIndex((e) => {
            return (e.dateDebut == dateDebut && e.dateFin == dateFin);
        });
        this.datesStatistic.splice(position, 1);
        await this.storage.set(this.DATE_STATISTIC_KEY, this.datesStatistic);
        this.getDatesStatistic();
    }
    async saveNetworkAddress(networkAddress: string) {
        await this.storage.set(this.NETWORK_ADDRESS_KEY, networkAddress);
        this.getNetworkAdress();
    }
    async saveDateStatistic(dateDebut, dateFin) {
        if (this.datesStatistic == null)
            console.log("--------------- null -----------");

        this.datesStatistic.push({
            dateDebut,
            dateFin
        });
        await this.storage.set(this.DATE_STATISTIC_KEY, this.datesStatistic);
        this.getDatesStatistic();
    }
    catchError(error: Response | any) {
        console.log(error);
        return Observable.throw(error.json().ExceptionMessage || "Server error");
    }
    logResponse(res: Response) {
        console.log(res);
    }
    extractData(res: Response) {
        return res.json();
    }

    showNotifSuccess(success: string) {

        let toast = this.toastCtrl.create({
            message: success,
            duration: 3000,
            position: 'bottom',
            cssClass: 'dark-trans',
            closeButtonText: 'OK',
            showCloseButton: true
        });
        toast.present();
    }
    showNotifError(error: string) {
        let toast = this.toastCtrl.create({
            message: 'Erreur ' + error,
            duration: 3000,
            position: 'bottom',
            cssClass: 'dark-trans',
            closeButtonText: 'OK',
            showCloseButton: true
        });
        toast.present();
    }
    createLink(...argume): string {
        let strUrl = this.networkAddress;
        argume.forEach(element => {
            strUrl += element + "/";
        });
        console.log("the utrl WITH CREATE LINK ",strUrl);      
        return strUrl;
    }
}
