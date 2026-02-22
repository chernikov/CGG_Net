import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-local-request-tab',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './local-request-tab.html',
  styleUrl: './local-request-tab.scss',
})
export class LocalRequestTabComponent {}
