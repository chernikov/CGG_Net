import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-observations-tab',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './observations-tab.html',
  styleUrl: './observations-tab.scss',
})
export class ObservationsTabComponent {}
