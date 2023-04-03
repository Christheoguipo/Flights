import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { PassengerService } from '../api/services';
import { AuthService } from '../auth/auth.service';
import { Router, ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-register-passenger',
  templateUrl: './register-passenger.component.html',
  styleUrls: ['./register-passenger.component.css']
})
export class RegisterPassengerComponent implements OnInit {

  constructor(private passengerService: PassengerService,
    private fb: FormBuilder,//to access values of the form
    private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  requestedUrl?: string =  undefined

  form = this.fb.group({
    email: ['', Validators.compose([Validators.required, Validators.email, Validators.minLength(3), Validators.maxLength(100)])],
    firstname: ['', Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(35)])],
    lastname: ['', Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(35)])],
    isfemale: [true, Validators.required]
  })

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(p => this.requestedUrl = p['requestedUrl']);
  }

  checkPassenger(): void {
    const params = { email: this.form.get('email')?.value! }

    this.passengerService.findPassenger(params)
      .subscribe(this.login,
        e => {
          if (e.status != 404)
            console.error(e);
        }
      )
  }

  register() {

    if (this.form.invalid)
      return;

    console.log(this.form.value)
    this.passengerService.registerPassenger({ body: this.form.value })
      .subscribe(this.login,
        console.error);
  }

  private login = () => {
    this.authService.loginUser({ email: this.form.get('email')?.value });
    this.router.navigate([this.requestedUrl ?? '/search-flights']);
  }

  get email() {
    return this.form.controls.email
  }

  get firstname() {
    return this.form.controls.firstname
  }

  get lastname() {
    return this.form.controls.lastname
  }

  get isfemale() {
    return this.form.controls.isfemale
  }

}
