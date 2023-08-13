import { Injectable } from '@angular/core';
import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private primary = '#bdbdbd';
  private secondary = '#444';
  private accent = '#111';
  private isGradientTheme = '0';

  private primary2 = '#9b2226';
  private secondary2 = '#001219';
  private accent2 = '#117a9b';

  constructor(private swagger: SwaggerClient) { }

  changeTheme() {
    [
      this.primary,
      this.secondary,
      this.accent,
      this.primary2,
      this.secondary2,
      this.accent2,
    ] = [
        this.primary2,
        this.secondary2,
        this.accent2,
        this.primary,
        this.secondary,
        this.accent,
      ];
  }

  getCurrentTheme(ThemeCode: string) {
    this.swagger
      .apiCommitteeMeetingSystemSettingSpecificThemeGet(ThemeCode)
      .subscribe((theme) => {

        this.primary = theme?.firstColorHex
          ? theme.firstColorHex
          : this.primary;

        this.secondary = theme?.secondColorHex
          ? theme.secondColorHex
          : this.secondary;

        this.accent = theme?.thirdColorHex ? theme.thirdColorHex : this.accent;
        this.isGradientTheme = theme?.isGradientTheme;
      });
  }

  get primaryBackground() {
    if (this.isGradientTheme == '1')
      return { 'background-image': `linear-gradient(to left, ${this.secondary}, ${this.accent})` };
    else return { backgroundColor: this.secondary, color: '#ffffff' };
  }

  get secondaryBackground() {
    return { backgroundColor: this.secondary };
  }

  get accentBackground() {
    return { backgroundColor: this.accent };
  }

  get primaryTextColor() {
    return { color: this.primary };
  }

  get secondaryTextColor() {
    return { color: this.secondary };
  }

  get accentTextColor() {
    return { color: this.accent };
  }

}
