//
//  ViewController.m
//  CrisisCheckin-iOS
//
//  Created by Stephanie Shupe on 10/5/13.
//  Copyright (c) 2013 Humanitarian Toolbox. All rights reserved.
//

#import "ViewController.h"

@interface ViewController ()

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    self.navigationController.navigationBar.barStyle = UIBarStyleBlack;
    [self setupNavigationItemTitleView];
}

- (void)setupNavigationItemTitleView {
    CGRect navigationBarFrame = self.navigationController.navigationBar.frame;
    CGFloat navigationBarWidth = navigationBarFrame.size.width;
    UIView *titleView = [[UIView alloc] initWithFrame:navigationBarFrame];
    [titleView addSubview:[self iconImageViewForTitleViewWithWidth:navigationBarWidth]];
    [titleView addSubview:[self titleLabelForTitleViewWithWidth:navigationBarWidth]];
    self.navigationItem.titleView = titleView;
}

- (UIImageView *)iconImageViewForTitleViewWithWidth:(CGFloat) navigationBarWidth {
    UIImageView *iconView = [[UIImageView alloc] initWithImage:[UIImage imageNamed:@"logo-red"]];
    iconView.frame = CGRectMake(0.5 * navigationBarWidth - 0.5 * 195, 5, 40, 40);
    iconView.contentMode = UIViewContentModeScaleAspectFit;
    return iconView;
}

- (UILabel *)titleLabelForTitleViewWithWidth:(CGFloat) navigationBarWidth {
    UILabel *titleLabel = [[UILabel alloc] initWithFrame:CGRectMake(0.5 * navigationBarWidth - 0.5 * 195 + 45, 5, 200, 40)];
    titleLabel.text = @"Crisis Check-in";
    titleLabel.textColor = [UIColor whiteColor];
    return titleLabel;
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
