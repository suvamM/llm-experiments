# demonstrate a simple computation graph

import torch
import torch.nn.functional as F
import torch

y = torch.tensor([1.0])
x1 = torch.tensor([1.1])
w1 = torch.tensor([2.2], requires_grad=True)
b = torch.tensor([3.3], requires_grad=True)
z = w1 * x1 + b
a = torch.sigmoid(z)
loss = F.binary_cross_entropy(a, y)

grad_L_w1 = torch.autograd.grad(loss, w1, retain_graph=True)
grad_L_b = torch.autograd.grad(loss, b, retain_graph=True)
print(f'grad_L_w1: {grad_L_w1}')
print(f'grad_L_b: {grad_L_b}')