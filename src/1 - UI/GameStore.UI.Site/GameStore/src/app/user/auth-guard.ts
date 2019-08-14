import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, Router } from '@angular/router';
import { UserService } from './user.service';
import { Observable } from 'rxjs/Rx';
import 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    isAdminLogged: boolean;

    constructor(private userService: UserService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.userService.getUserInformation().map(e => {
            if (e.roles.indexOf('Admin') !== -1) {
                return true;
            } else {
                this.router.navigate(['/login']);
            }
        }).catch(() => {
            return Observable.of(false);
        });
    }
}