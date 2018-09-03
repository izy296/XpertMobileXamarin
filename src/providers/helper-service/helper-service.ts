import { Storage } from '@ionic/storage';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
@Injectable()
export class HelperServiceProvider {
    NETWORK_ADDRESS_KEY: string = 'network_address';
    DATE_STATISTIC_KEY: string = 'Date_Statistic';
    networkAddress: string;
    datesStatistic = new Array();
    public constructor(private storage: Storage) {
        this.getDatesStatistic();
    }
    async getNetworkAdress() {
        await this.storage.get(this.NETWORK_ADDRESS_KEY).then((networkAddresse) => {
            this.networkAddress = networkAddresse;
            if (this.networkAddress == null) {
                this.saveNetworkAddress("http://localhost/");
            }
        }, (error) => {
            console.log(error);
        });
        return this.networkAddress;
    }
    getDatesStatistic() {
        this.storage.get(this.DATE_STATISTIC_KEY).then((dates) => {
            if (dates != null) {
                this.datesStatistic = dates;
            }
        }, (error) => {
            console.log(error);
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
        console.log("datastartistic", this.datesStatistic);
        await this.storage.set(this.DATE_STATISTIC_KEY, this.datesStatistic);
        this.getDatesStatistic();
    }
    catchError(error: Response | any) {
        console.log(error);
        return Observable.throw(error.json().error || "Server error");
    }
    logResponse(res: Response) {
        console.log(res);
    }
    extractData(res: Response) {
        return res.json();
    }

}